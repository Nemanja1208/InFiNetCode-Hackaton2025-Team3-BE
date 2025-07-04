namespace Domain_Layer.Models
{
    public class IdeaSession
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; } = "in_progress";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Step> Steps { get; set; }
        public ICollection<MvpPlan> MvpPlans { get; set; }
        public ICollection<TechRecommendation> TechRecommendations { get; set; }
    }
}
