using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application_Layer.IdeaSessions.Dto;


namespace Application_Layer.IdeaSessions.Dto
{
    public class IdeaSessionDto
    {
        public Guid IdeaId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
