using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Application-Layer/Features/IdeaSessions/Queries/GetByUser/GetIdeaSessionsByUserQueryValidator.cs
using Application.Common.Behaviors;
using Application_Layer.Features.IdeaSessions.Queries.GetByUser;

namespace Application_Layer.Features.IdeaSessions.Queries.GetByUser
{
    public class GetIdeaSessionsByUserQueryValidator
        : ICustomValidator<GetIdeaSessionsByUserQuery>
    {
        public void Validate(GetIdeaSessionsByUserQuery request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Query cannot be null.");
            }
            // No additional properties to validate as UserId is obtained from ICurrentUserService.
        }
    }
}
