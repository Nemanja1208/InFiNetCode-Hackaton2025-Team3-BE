using Application_Layer.Common.Interfaces;
using Domain_Layer.Models;
using MediatR;


namespace Application_Layer.StepTemplates.Commands.SeedStepTemplates
{
    public class SeedStepTemplatesCommandHandler : IRequestHandler<SeedStepTemplatesCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<StepTemplate> _stepTemplateRepository;

        public SeedStepTemplatesCommandHandler(IGenericRepository<StepTemplate> stepTemplateRepository)
        {
            _stepTemplateRepository = stepTemplateRepository;
        }

        public async Task<OperationResult<string>> Handle(SeedStepTemplatesCommand request, CancellationToken cancellationToken)
        {
            // Check if templates already exist to prevent duplicates
            var existingTemplates = await _stepTemplateRepository.GetAllAsync();
            if (existingTemplates.Any())
            {
                return OperationResult<string>.Success("Step templates already exist. No new templates seeded.");
            }

            var defaultTemplates = new List<StepTemplate>
            {
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Project Goal",
                    Question = "What is the main goal or purpose of your MVP?",
                    AiPromptTemplate = "Understand the core goal: {UserInput}",
                    Order = 1
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Target Users",
                    Question = "Who are the primary users or target audience for your MVP?",
                    AiPromptTemplate = "Identify target audience: {UserInput}",
                    Order = 2
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Core Functionality",
                    Question = "What are the absolute minimum core features your MVP must have to achieve its goal?",
                    AiPromptTemplate = "List essential features: {UserInput}",
                    Order = 3
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "User Experience",
                    Question = "Describe the desired user experience or key interactions within your MVP.",
                    AiPromptTemplate = "Outline UX/UI considerations: {UserInput}",
                    Order = 4
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Development Experience",
                    Question = "What is your current development experience level (e.g., beginner, intermediate, expert) and what programming languages/frameworks are you familiar with?",
                    AiPromptTemplate = "Assess user's development background: {UserInput}",
                    Order = 5
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Scalability Needs",
                    Question = "Do you anticipate high user traffic or data volume in the future? (e.g., low, medium, high)",
                    AiPromptTemplate = "Evaluate scalability requirements: {UserInput}",
                    Order = 6
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Budget & Resources",
                    Question = "What is your approximate budget or available resources for development and hosting?",
                    AiPromptTemplate = "Consider budget and resource constraints: {UserInput}",
                    Order = 7
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Monetization Strategy",
                    Question = "How do you plan to monetize your MVP, if applicable?",
                    AiPromptTemplate = "Analyze monetization strategy: {UserInput}",
                    Order = 8
                },
                new StepTemplate
                {
                    Id = Guid.NewGuid(),
                    Title = "Unique Selling Proposition",
                    Question = "What makes your MVP unique or different from existing solutions?",
                    AiPromptTemplate = "Identify unique value proposition: {UserInput}",
                    Order = 9
                }
            };

            foreach (var template in defaultTemplates)
            {
                await _stepTemplateRepository.CreateAsync(template);
            }

            return OperationResult<string>.Success("Default step templates seeded successfully.");
        }
    }
}
