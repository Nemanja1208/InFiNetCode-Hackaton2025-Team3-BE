using System;

namespace Domain_Layer.Models
{
    public class MvpPlan
    {
        public Guid Id { get; set; }
        public Guid IdeaSessionId { get; set; }
        public string Summary { get; set; }
        public string TargetAudience { get; set; }
        public string KeyFeatures { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public IdeaSession IdeaSession { get; set; }
        public ICollection<UserStory> UserStories { get; set; }
    }
}
