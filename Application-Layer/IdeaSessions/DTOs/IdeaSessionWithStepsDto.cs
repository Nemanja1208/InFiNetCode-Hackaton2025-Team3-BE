using System.Collections.Generic;

namespace Application_Layer.IdeaSessions.DTOs;

public class IdeaSessionWithStepsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public List<StepDto> Steps { get; set; } = new();
    public List<MvpPlanDto> MvpPlans { get; set; } = new();
}
