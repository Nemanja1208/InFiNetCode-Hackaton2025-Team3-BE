using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application_Layer.IdeaSessions.Dto;


namespace Application_Layer.IdeaSessions.Commands
{
    public class CreateIdeaSessionCommand : IRequest<IdeaSessionDto>
    {
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public object Description { get; set; }

        public CreateIdeaSessionCommand(string title, Guid userId, object description)
        {
            Title = title;
            UserId = userId;
            Description = description;
            
            
        }
    }
}


