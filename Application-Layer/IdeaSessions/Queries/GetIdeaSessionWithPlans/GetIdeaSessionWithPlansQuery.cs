using Application_Layer.IdeaSessions.DTOs;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.IdeaSessions.Queries.GetIdeaSessionWithPlans
{
    public record GetIdeaSessionWithPlansQuery(Guid IdeaSessionId, Guid UserId) : IRequest<IdeaSessionWithStepsDto?>;
}
