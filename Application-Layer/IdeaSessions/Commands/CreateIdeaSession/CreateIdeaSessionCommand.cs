using Application_Layer.IdeaSessions.DTOs;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.IdeaSessions.Commands.CreateIdeaSession
{
    public record CreateIdeaSessionCommand(CreateIdeaSessionDto Dto, Guid UserId) : IRequest<OperationResult<IdeaSessionDto>>;
}
