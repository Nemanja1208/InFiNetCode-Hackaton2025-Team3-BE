using Application_Layer.IdeaSessions.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application_Layer.Features.IdeaSessions.Queries.GetByUser
{
    public class GetIdeaSessionsByUserQuery : IRequest<IEnumerable<IdeaSessionWithStepsDto>>
    {
        // Inga properties – UserId hämtas från ICurrentUserService
    }
}
