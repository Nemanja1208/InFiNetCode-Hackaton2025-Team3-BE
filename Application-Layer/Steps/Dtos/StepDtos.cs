﻿
namespace Application_Layer.Steps.Dtos
{
    public class CreateStepDto
    {
        public Guid IdeaSessionId { get; set; }
        public Guid StepTemplateId { get; set; }
        public string UserInput { get; set; }
        public string AiResponse { get; set; }
        public int Order { get; set; }
    }
}

