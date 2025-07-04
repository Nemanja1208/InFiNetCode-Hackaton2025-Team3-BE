namespace Application_Layer.IdeaSessions.DTOs;

public class StepDto
{
    public Guid StepId { get; set; }
    public Guid StepTemplateId { get; set; }
    public string? UserInput { get; set; }
    public int Order { get; set; }
}
