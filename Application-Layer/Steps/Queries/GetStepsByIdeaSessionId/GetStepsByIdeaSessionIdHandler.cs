using Application_Layer.Common;
using Application_Layer.Common.Interfaces;
using Application_Layer.Steps.Interfaces;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.Steps.Queries.GetStepsByIdeaSessionId
{
    public class GetStepsByIdeaSessionIdHandler : IRequestHandler<GetStepsByIdeaSessionIdQuery, OperationResult<List<StepResponseDto>>>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IGenericRepository<IdeaSession> _ideaSessionRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public GetStepsByIdeaSessionIdHandler(IStepRepository stepRepository, 
                                              IMapper mapper, 
                                              IGenericRepository<IdeaSession> ideaSessionRepository, 
                                              IUserContextService userContextService)
        {
            _stepRepository = stepRepository;
            _ideaSessionRepository = ideaSessionRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<StepResponseDto>>> Handle(GetStepsByIdeaSessionIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return OperationResult<List<StepResponseDto>>.Failure("User Id not found.");

            var ideaSession = await _ideaSessionRepository.GetByIdAsync(request.IdeaSessionId);
            if (ideaSession is null)
            {
                return OperationResult<List<StepResponseDto>>.Failure($"Idea Session with Id {request.IdeaSessionId} not found.");
            }

            if (ideaSession.UserId.ToString() != userId)
            {
                return OperationResult<List<StepResponseDto>>.Failure("You do not have permission to access this Idea Session.");
            }

            var steps = await _stepRepository.GetStepsByIdeaSessionId(request.IdeaSessionId);
            if (steps is null)
            {
                steps = [];
            }
            var stepDtos = _mapper.Map<List<StepResponseDto>>(steps);
            return OperationResult<List<StepResponseDto>>.Success(stepDtos);
        }
    }
}
