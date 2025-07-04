namespace Application_Layer.IdeaSessions.DTOs
{
    public class MvpPlanDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public string? ExperienceLevel { get; set; }
        public string ProblemStatement { get; set; } = string.Empty;
        public string SolutionApproach { get; set; } = string.Empty;
        public string ValueProposition { get; set; } = string.Empty;
        public string PrimaryTargetAudience { get; set; } = string.Empty;
        public string SecondaryTargetAudience { get; set; } = string.Empty;
        public string KeyFeatures { get; set; } = string.Empty;
        public string TechnicalStack { get; set; } = string.Empty;
        public string EstimatedBudget { get; set; } = string.Empty;
        public string TimelineEstimate { get; set; } = string.Empty;
        public string MonetizationStrategy { get; set; } = string.Empty;
        public string NextSteps { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
