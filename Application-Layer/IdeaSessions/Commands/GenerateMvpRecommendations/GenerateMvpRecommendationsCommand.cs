using MediatR;
using System;
using Domain_Layer.Models;
using Application_Layer.IdeaSessions.DTOs;

namespace Application_Layer.IdeaSessions.Commands.GenerateMvpRecommendations
{
    public record GenerateMvpRecommendationsCommand(Guid IdeaSessionId, Guid UserId) : IRequest<OperationResult<IdeaSessionDto>>;
}
