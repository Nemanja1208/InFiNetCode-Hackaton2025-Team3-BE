using Application_Layer.Common.Interfaces;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.IdeaSessions.Commands.CreateIdeaSession
{
    public class CreateIdeaSessionCommandHandler : IRequestHandler<CreateIdeaSessionCommand, OperationResult<IdeaSessionDto>>
    {
        private readonly IGenericRepository<IdeaSession> _ideaSessionRepository;
        private readonly IGenericRepository<MvpPlan> _mvpPlanRepository;
        private readonly IMapper _mapper;

        public CreateIdeaSessionCommandHandler(IGenericRepository<IdeaSession> ideaSessionRepository, 
                                               IGenericRepository<MvpPlan> mvpPlanRepository, 
                                               IMapper mapper)
        {
            _ideaSessionRepository = ideaSessionRepository;
            _mvpPlanRepository = mvpPlanRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IdeaSessionDto>> Handle(CreateIdeaSessionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1) Map DTO from command to entity
                var ideaSession = _mapper.Map<IdeaSession>(request.Dto);
                var mvpPlan = _mapper.Map<MvpPlan>(request.Dto);

                // Set generated properties
                ideaSession.Id = Guid.NewGuid();
                ideaSession.CreatedAt = DateTime.UtcNow;
                // UserId is obtained from the authenticated user and passed directly to the command.
                ideaSession.UserId = request.UserId;

                mvpPlan.Id = Guid.NewGuid();
                mvpPlan.IdeaSessionId = ideaSession.Id;

                // 2) Save to database
                await _ideaSessionRepository.CreateAsync(ideaSession);
                await _mvpPlanRepository.CreateAsync(mvpPlan);

                // 3) Return success with mapped DTO
                var ideaSessionDto = _mapper.Map<IdeaSessionDto>(ideaSession);
                return OperationResult<IdeaSessionDto>.Success(ideaSessionDto);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return OperationResult<IdeaSessionDto>.Failure($"Failed to create idea session: {ex.Message}");
            }
        }
    }
}
