using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application_Layer.IdeaSessions.Dto //Vi Threading emot data från frontend 
{
    public class CreateIdeaSessionDto
    {
        public string Title { get; set; }
        public object Description { get; set; }
        public object userId { get; set; }
        public Guid IdeaSessionId { get; set; }
    }
}