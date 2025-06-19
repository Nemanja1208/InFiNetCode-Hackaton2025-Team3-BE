using System;
using System.Collections.Generic;

namespace Domain_Layer.Models
{
    public class StepTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string AiPromptTemplate { get; set; }
        public int Order { get; set; }

        // Navigation properties
        public ICollection<Step> Steps { get; set; }
    }
}
