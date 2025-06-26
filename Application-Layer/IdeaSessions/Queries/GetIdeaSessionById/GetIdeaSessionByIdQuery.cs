using Application_Layer.IdeaSessions.DTOs;
using MediatR;

namespace Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;

public class GetIdeaSessionByIdQuery : IRequest<IdeaSessionWithStepsDto?>
{
    public Guid IdeaId { get; }
    public Guid UserId { get; }

    public GetIdeaSessionByIdQuery(Guid ideaId, Guid userId)
    {
        IdeaId = ideaId;
        UserId = userId;
    }
}
