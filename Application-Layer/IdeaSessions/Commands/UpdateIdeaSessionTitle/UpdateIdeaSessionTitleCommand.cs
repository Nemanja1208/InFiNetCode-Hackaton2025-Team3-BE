using MediatR;
using Domain_Layer.Models;

namespace Application_Layer.IdeaSessions.Commands.UpdateIdeaSessionTitle;

public class UpdateIdeaSessionTitleCommand : IRequest<IdeaSession?>
{
    public Guid IdeaSessionId { get; set; }
    public string Title { get; set; } = default!;
    public Guid UserId { get; set; }
}
