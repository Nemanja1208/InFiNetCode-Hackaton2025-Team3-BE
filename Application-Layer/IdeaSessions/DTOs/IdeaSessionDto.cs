namespace Application_Layer.IdeaSessions.DTOs
{
    public class IdeaSessionDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public List<MvpPlanDto>? MvpPlans { get; set; }
    }

    public class MvpPlanDto
    {
        public Guid Id { get; set; }
        public required string Summary { get; set; }
        public required string TargetAudience { get; set; }
        public required string KeyFeatures { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
