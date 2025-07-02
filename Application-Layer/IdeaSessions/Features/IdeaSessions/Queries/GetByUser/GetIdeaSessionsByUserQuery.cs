using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application_Layer.IdeaSessions.DTOs;
using MediatR;

namespace Application_Layer.IdeaSessions.Features.IdeaSessions.Queries.GetByUser
{
    public class GetIdeaSessionsByUserQuery : IRequest<IEnumerable<IdeaSessionWithStepsDto>>
    {
        // Inga properties – UserId hämtas från ICurrentUserService
    }
}