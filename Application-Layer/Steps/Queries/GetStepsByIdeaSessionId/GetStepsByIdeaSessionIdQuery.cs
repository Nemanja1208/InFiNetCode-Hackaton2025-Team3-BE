using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.Steps.Queries.GetStepsByIdeaSessionId
{
    public record GetStepsByIdeaSessionIdQuery(Guid IdeaSessionId) : IRequest<OperationResult<List<StepResponseDto>>>;
}
