namespace Application_Layer.IdeaSessions.DTOs;

public class StepDto
{
    public Guid StepId { get; set; }
    public int StepTemplateId { get; set; }
    public string? UserInput { get; set; }
    public string? AiResponse { get; set; }
    public int StepOrder { get; set; }
}
