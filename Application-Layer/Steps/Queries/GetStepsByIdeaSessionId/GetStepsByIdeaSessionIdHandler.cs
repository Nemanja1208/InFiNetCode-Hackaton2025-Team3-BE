using Application_Layer.Common.Interfaces;
using Application_Layer.Steps.Interfaces;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application_Layer.Steps.Dtos; // Assuming StepResponseDto is here

namespace Application_Layer.Steps.Queries.GetStepsByIdeaSessionId
{
    public class GetStepsByIdeaSessionIdHandler : IRequestHandler<GetStepsByIdeaSessionIdQuery, OperationResult<List<StepResponseDto>>>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IGenericRepository<IdeaSession> _ideaSessionRepository;
        private readonly IMapper _mapper;

        public GetStepsByIdeaSessionIdHandler(IStepRepository stepRepository,
                                              IMapper mapper,
                                              IGenericRepository<IdeaSession> ideaSessionRepository)
        {
            _stepRepository = stepRepository;
            _ideaSessionRepository = ideaSessionRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<StepResponseDto>>> Handle(GetStepsByIdeaSessionIdQuery request, CancellationToken cancellationToken)
        {
            // Note: User ID validation should ideally happen at a higher level (e.g., controller or authorization middleware)
            // or be passed into the query/command if needed for business logic.
            // For now, assuming the query is for publicly accessible steps or steps where ownership is not strictly enforced here.

            var ideaSession = await _ideaSessionRepository.GetByIdAsync(request.IdeaSessionId);
            if (ideaSession is null)
            {
                return OperationResult<List<StepResponseDto>>.Failure($"Idea Session with Id {request.IdeaSessionId} not found.");
            }

            // If user ID validation is needed, it should be passed in the query and checked here.
            // For example: if (ideaSession.UserId != request.UserId) return OperationResult<List<StepResponseDto>>.Failure("Unauthorized access.");

            var steps = await _stepRepository.GetStepsByIdeaSessionId(request.IdeaSessionId);
            if (steps is null)
            {
                steps = new List<Step>();
            }
            var stepDtos = _mapper.Map<List<StepResponseDto>>(steps);
            return OperationResult<List<StepResponseDto>>.Success(stepDtos);
        }
    }
}
