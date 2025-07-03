using Application_Layer.Common.Interfaces;
using Application_Layer.Steps.Dtos;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.Steps.Commands.CreateStep
{
    public class CreateStepCommandHandler : IRequestHandler<CreateStepCommand, OperationResult<CreateStepResponseDto>>
    {
        private readonly IGenericRepository<Step> _repository;
        private readonly IGenericRepository<StepTemplate> _stepTemplateRepository;
        private readonly IGenericRepository<IdeaSession> _ideaSessionRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateStepCommandHandler(
            IGenericRepository<Step> repo,
            IGenericRepository<StepTemplate> stepTemplateRepo,
            IGenericRepository<IdeaSession> ideaSessionRepo,
            IMediator mediator,
            IMapper mapper)
        {
            _repository = repo;
            _stepTemplateRepository = stepTemplateRepo;
            _ideaSessionRepository = ideaSessionRepo;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<OperationResult<CreateStepResponseDto>> Handle(CreateStepCommand request, CancellationToken cancellationToken)
        {
            // Find existing Step for this IdeaSession and StepTemplate
            var allSteps = await _repository.GetAllAsync();
            var existingStep = allSteps.FirstOrDefault(s =>
                s.IdeaSessionId == request.Dto.IdeaSessionId &&
                s.StepTemplateId == request.Dto.StepTemplateId);

            if (existingStep != null)
            {
                // Update existing Step
                existingStep.UserInput = request.Dto.UserInput;
                existingStep.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(existingStep);

                // Check if all steps are now answered and auto-trigger recommendations
                await CheckAndTriggerRecommendationsAsync(existingStep.IdeaSessionId, cancellationToken);

                return OperationResult<CreateStepResponseDto>.Success(_mapper.Map<CreateStepResponseDto>(existingStep));
            }
            else
            {
                // Create new Step (fallback - shouldn't happen with auto-created Steps)
                var step = _mapper.Map<Step>(request.Dto);
                step.Id = Guid.NewGuid();
                step.CreatedAt = DateTime.UtcNow;
                step.UpdatedAt = DateTime.UtcNow;

                // Get the StepTemplate to use its Order
                var stepTemplate = await _stepTemplateRepository.GetByIdAsync(step.StepTemplateId);
                if (stepTemplate != null)
                {
                    step.Order = stepTemplate.Order;
                }
                else
                {
                    step.Order = 0; // Fallback
                }

                await _repository.CreateAsync(step);

                // Check if all steps are now answered and auto-trigger recommendations
                await CheckAndTriggerRecommendationsAsync(step.IdeaSessionId, cancellationToken);

                return OperationResult<CreateStepResponseDto>.Success(_mapper.Map<CreateStepResponseDto>(step));
            }
        }

        private async Task CheckAndTriggerRecommendationsAsync(Guid ideaSessionId, CancellationToken cancellationToken)
        {
            // Get all steps for this IdeaSession
            var allSteps = await _repository.GetAllAsync();
            var sessionSteps = allSteps.Where(s => s.IdeaSessionId == ideaSessionId).ToList();

            // Check if all steps have UserInput (are answered)
            var allAnswered = sessionSteps.All(s => !string.IsNullOrWhiteSpace(s.UserInput));

            if (allAnswered && sessionSteps.Any())
            {
                // Get the IdeaSession to get UserId
                var ideaSession = await _ideaSessionRepository.GetByIdAsync(ideaSessionId);
                if (ideaSession != null)
                {
                    // All questions are answered - trigger MVP recommendations
                    var generateCommand = new Application_Layer.IdeaSessions.Commands.GenerateMvpRecommendations.GenerateMvpRecommendationsCommand(
                        ideaSessionId,
                        ideaSession.UserId
                    );

                    try
                    {
                        await _mediator.Send(generateCommand, cancellationToken);
                    }
                    catch (Exception)
                    {
                        // Log error but don't fail the step creation
                        // The user can manually trigger recommendations later
                    }
                }
            }
        }
    }
}
