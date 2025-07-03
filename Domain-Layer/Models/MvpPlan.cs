using System;

namespace Domain_Layer.Models
{
        public class MvpPlan
        {
                public Guid Id { get; set; }
                public Guid IdeaSessionId { get; set; }

                // Core Information
                public string? Summary { get; set; }
                public string Goal { get; set; } = string.Empty;
                public string Title { get; set; } = string.Empty;
                public string ProblemStatement { get; set; } = string.Empty;
                public string SolutionApproach { get; set; } = string.Empty;
                public string ValueProposition { get; set; } = string.Empty;

                // Target Audience
                public string PrimaryTargetAudience { get; set; } = string.Empty;
                public string SecondaryTargetAudience { get; set; } = string.Empty;

                // Features & Technical
                public string KeyFeatures { get; set; } = string.Empty;
                public string TechnicalStack { get; set; } = string.Empty;

                // Business Planning
                public string? ExperienceLevel { get; set; }
                public string EstimatedBudget { get; set; } = string.Empty;
                public string TimelineEstimate { get; set; } = string.Empty;
                public string MonetizationStrategy { get; set; } = string.Empty;

                // Action Items
                public string NextSteps { get; set; } = string.Empty;

                // Backup of full AI response
                public string RawAiResponse { get; set; } = string.Empty;

                public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

                // Navigation properties
                public IdeaSession IdeaSession { get; set; } = null!;
                public ICollection<UserStory> UserStories { get; set; } = new List<UserStory>();
        }
}
