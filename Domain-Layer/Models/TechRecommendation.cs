using System;

namespace Domain_Layer.Models
{
    public class TechRecommendation
    {
        public Guid Id { get; set; }
        public Guid IdeaSessionId { get; set; }
        public string Technologies { get; set; }
        public string Reasoning { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public IdeaSession IdeaSession { get; set; }
    }
}
