using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application_Layer.IdeaSessions.Features.IdeaSessions.Queries.GetByUser
{    public class GetIdeaSessionsByUserQueryValidator
        : AbstractValidator<GetIdeaSessionsByUserQuery>
    {
        public static void Validate(GetIdeaSessionsByUserQuery request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Query cannot be null.");
            }
        }
    }
}