
namespace Application_Layer.StepTemplates.Dtos
{
    public class StepTemplateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string AiPromptTemplate { get; set; }
        public int Order { get; set; }
    }

    public class CreateStepTemplateDto
    {
        public string Title { get; set; }
        public string Question { get; set; }
        public string AiPromptTemplate { get; set; }
        public int Order { get; set; }
    }
}