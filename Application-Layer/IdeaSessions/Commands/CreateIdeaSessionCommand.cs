using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application_Layer.IdeaSessions.Dto;
using System;

namespace Application_Layer.IdeaSessions.Commands
{
    public class CreateIdeaSessionCommand : IRequest<IdeaSessionDto>
    {
        public string Title { get; set; }
        public Guid UserId { get; set; }
    }
}