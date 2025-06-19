using System;

namespace Domain_Layer.Models
{
    public class UserStory
    {
        public Guid Id { get; set; }
        public Guid MvpPlanId { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public MvpPlan MvpPlan { get; set; }
    }
}
