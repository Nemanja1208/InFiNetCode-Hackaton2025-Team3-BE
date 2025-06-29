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
        private object description;

        public CreateIdeaSessionCommand(string Title, object Description, Guid UserId)
        {
            this.Title = Title;
            description = Description;
            this.UserId = UserId;
        }

        public string Title { get; set; }
        public Guid UserId { get; set; }
    }
}