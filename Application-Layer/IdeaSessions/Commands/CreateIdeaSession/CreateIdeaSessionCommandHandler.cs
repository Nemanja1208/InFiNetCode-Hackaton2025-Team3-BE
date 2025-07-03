using Application_Layer.Common.Interfaces;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.IdeaSessions.Commands.CreateIdeaSession
{
    public class CreateIdeaSessionCommandHandler : IRequestHandler<CreateIdeaSessionCommand, OperationResult<IdeaSessionDto>>
    {
        private readonly IGenericRepository<IdeaSession> _repo;
        private readonly IGenericRepository<StepTemplate> _stepTemplateRepo;
        private readonly IGenericRepository<Step> _stepRepo;
        private readonly IMapper _mapper;

        public CreateIdeaSessionCommandHandler(
            IGenericRepository<IdeaSession> repo,
            IGenericRepository<StepTemplate> stepTemplateRepo,
            IGenericRepository<Step> stepRepo,
            IMapper mapper)
        {
            _repo = repo;
            _stepTemplateRepo = stepTemplateRepo;
            _stepRepo = stepRepo;
            _mapper = mapper;
        }

        public async Task<OperationResult<IdeaSessionDto>> Handle(CreateIdeaSessionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1) Map DTO from command to entity
                var ideaSession = _mapper.Map<IdeaSession>(request.Dto);

                // Set generated properties
                ideaSession.Id = Guid.NewGuid();
                ideaSession.CreatedAt = DateTime.UtcNow;
                // UserId is obtained from the authenticated user and passed directly to the command.
                ideaSession.UserId = request.UserId;

                // 2) Save to database
                await _repo.CreateAsync(ideaSession);

                // 3) Auto-create Steps for all StepTemplates
                await CreateStepsForIdeaSessionAsync(ideaSession.Id);

                // 4) Return success with mapped DTO
                var ideaSessionDto = _mapper.Map<IdeaSessionDto>(ideaSession);
                return OperationResult<IdeaSessionDto>.Success(ideaSessionDto);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return OperationResult<IdeaSessionDto>.Failure($"Failed to create idea session: {ex.Message}");
            }
        }

        private async Task CreateStepsForIdeaSessionAsync(Guid ideaSessionId)
        {
            // Get all StepTemplates ordered by their Order
            var stepTemplates = await _stepTemplateRepo.GetAllAsync();
            var orderedTemplates = stepTemplates.OrderBy(st => st.Order).ToList();

            // Create a Step for each StepTemplate
            var steps = orderedTemplates.Select(template => new Step
            {
                Id = Guid.NewGuid(),
                IdeaSessionId = ideaSessionId,
                StepTemplateId = template.Id,
                UserInput = null, // Empty initially - user will fill this
                Order = template.Order,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            // Save all Steps to database
            foreach (var step in steps)
            {
                await _stepRepo.CreateAsync(step);
            }
        }
    }
}
