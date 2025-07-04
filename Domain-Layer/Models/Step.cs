using System;

namespace Domain_Layer.Models
{
    public class Step
    {
        public Guid Id { get; set; }
        public Guid IdeaSessionId { get; set; }
        public Guid StepTemplateId { get; set; }
        public string? UserInput { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public IdeaSession IdeaSession { get; set; }
        public StepTemplate StepTemplate { get; set; }
    }
}
