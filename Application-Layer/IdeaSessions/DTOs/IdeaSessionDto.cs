using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application_Layer.IdeaSessions.Dto
{
    public class IdeaSessionDto //retunera reslutat 
    {
        public object Id;
        public Guid IdeaId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public object IdeaSessionId { get; set; }
    }
}