
namespace Application_Layer.Steps.Dtos
{
    public class CreateStepRequestDto
    {
        public Guid IdeaSessionId { get; set; }
        public Guid StepTemplateId { get; set; }
        public string UserInput { get; set; }
        public string AiResponse { get; set; }
        public int Order { get; set; }
    }

    public class CreateStepResponseDto
    {
        public Guid Id { get; set; }
        public Guid IdeaSessionId { get; set; }
        public Guid StepTemplateId { get; set; }
        public string UserInput { get; set; }
        public string AiResponse { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

