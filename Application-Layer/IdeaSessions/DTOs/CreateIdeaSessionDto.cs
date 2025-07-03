namespace Application_Layer.IdeaSessions.DTOs
{
    public class CreateIdeaSessionDto
    {
        public required string Title { get; set; }
        public string Purpose { get; set; } 
        public string Goal { get; set; }
        public string TargetAudience { get; set; }
        public string? ExperienceLevel { get; set; }
        public string TimeEstimate { get; set; }
        public string? KeyFeatures { get; set; }
    }
}
