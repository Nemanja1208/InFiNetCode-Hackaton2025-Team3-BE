using Application_Layer.Common.Interfaces;
using Domain_Layer.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Services
{
    public class AiMvpPlannerService : IAiMvpPlannerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AiMvpPlannerService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<MvpPlan> GenerateMvpPlanAsync(IdeaSession ideaSession)
        {
            var apiKey = _configuration["GeminiSettings:ApiKey"];
            var modelName = _configuration["GeminiSettings:ModelName"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(modelName))
            {
                throw new InvalidOperationException("Gemini API configuration is missing. Please check GeminiSettings in appsettings.");
            }

            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={apiKey}";

            // Build comprehensive prompt for Gemini
            var prompt = new StringBuilder();
            prompt.AppendLine($"You are an expert MVP (Minimum Viable Product) consultant. Generate a comprehensive MVP plan for the following project:");
            prompt.AppendLine($"Project Title: {ideaSession.Title}");
            prompt.AppendLine();
            prompt.AppendLine("Based on the user's detailed responses to key questions:");

            foreach (var step in ideaSession.Steps)
            {
                prompt.AppendLine($"Q: {step.StepTemplate.Question}");
                prompt.AppendLine($"A: {step.UserInput}");
                prompt.AppendLine();
            }

            prompt.AppendLine("Please provide a detailed MVP plan with the following structure:");
            prompt.AppendLine("1. TITLE: A catchy, descriptive project name");
            prompt.AppendLine("2. PROBLEM STATEMENT: Clear description of the problem being solved");
            prompt.AppendLine("3. SOLUTION APPROACH: How the MVP will solve this problem");
            prompt.AppendLine("4. VALUE PROPOSITION: What unique value this MVP provides");
            prompt.AppendLine("5. PRIMARY TARGET AUDIENCE: Main user group");
            prompt.AppendLine("6. SECONDARY TARGET AUDIENCE: Secondary user group (if applicable)");
            prompt.AppendLine("7. CORE FEATURES: Essential features for MVP");
            prompt.AppendLine("8. TECHNICAL STACK: Recommended technologies");
            prompt.AppendLine("9. ESTIMATED BUDGET: Budget range estimate");
            prompt.AppendLine("10. TIMELINE ESTIMATE: Development timeline");
            prompt.AppendLine("11. MONETIZATION STRATEGY: How to generate revenue");
            prompt.AppendLine("12. NEXT STEPS: Immediate action items");
            prompt.AppendLine();
            prompt.AppendLine("Make the response practical, actionable, and tailored specifically to the user's answers. Focus on creating a realistic MVP that can be built and launched quickly while providing real value to users.");

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt.ToString() }
                        }
                    }
                }
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Gemini API returned {response.StatusCode}: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(jsonResponse);

                // Extract the AI-generated content
                var aiContent = geminiResponse?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text ?? "";

                if (string.IsNullOrEmpty(aiContent))
                {
                    throw new InvalidOperationException("Gemini API returned empty content");
                }

                // Parse the AI response into structured MVP plan
                var parsedPlan = ParseAiResponseToMvpPlan(aiContent, ideaSession.Id);

                return parsedPlan;
            }
            catch (Exception ex)
            {
                // Re-throw with more context instead of using fallback
                throw new InvalidOperationException($"AI service failed to generate MVP plan: {ex.Message}. Please check your API configuration and try again.", ex);
            }
        }


        private MvpPlan ParseAiResponseToMvpPlan(string aiContent, Guid ideaSessionId)
        {
            var lines = aiContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var sections = new Dictionary<string, StringBuilder>
            {
                ["title"] = new StringBuilder(),
                ["problem"] = new StringBuilder(),
                ["solution"] = new StringBuilder(),
                ["value"] = new StringBuilder(),
                ["primary"] = new StringBuilder(),
                ["secondary"] = new StringBuilder(),
                ["features"] = new StringBuilder(),
                ["tech"] = new StringBuilder(),
                ["budget"] = new StringBuilder(),
                ["timeline"] = new StringBuilder(),
                ["monetization"] = new StringBuilder(),
                ["nextsteps"] = new StringBuilder()
            };

            var currentSection = "";

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                // Identify sections based on keywords
                if (trimmedLine.ToUpper().Contains("TITLE") || trimmedLine.StartsWith("1."))
                    currentSection = "title";
                else if (trimmedLine.ToUpper().Contains("PROBLEM STATEMENT") || trimmedLine.StartsWith("2."))
                    currentSection = "problem";
                else if (trimmedLine.ToUpper().Contains("SOLUTION APPROACH") || trimmedLine.StartsWith("3."))
                    currentSection = "solution";
                else if (trimmedLine.ToUpper().Contains("VALUE PROPOSITION") || trimmedLine.StartsWith("4."))
                    currentSection = "value";
                else if (trimmedLine.ToUpper().Contains("PRIMARY TARGET") || trimmedLine.StartsWith("5."))
                    currentSection = "primary";
                else if (trimmedLine.ToUpper().Contains("SECONDARY TARGET") || trimmedLine.StartsWith("6."))
                    currentSection = "secondary";
                else if (trimmedLine.ToUpper().Contains("CORE FEATURES") || trimmedLine.StartsWith("7."))
                    currentSection = "features";
                else if (trimmedLine.ToUpper().Contains("TECHNICAL STACK") || trimmedLine.StartsWith("8."))
                    currentSection = "tech";
                else if (trimmedLine.ToUpper().Contains("ESTIMATED BUDGET") || trimmedLine.StartsWith("9."))
                    currentSection = "budget";
                else if (trimmedLine.ToUpper().Contains("TIMELINE ESTIMATE") || trimmedLine.StartsWith("10."))
                    currentSection = "timeline";
                else if (trimmedLine.ToUpper().Contains("MONETIZATION") || trimmedLine.StartsWith("11."))
                    currentSection = "monetization";
                else if (trimmedLine.ToUpper().Contains("NEXT STEPS") || trimmedLine.StartsWith("12."))
                    currentSection = "nextsteps";
                else if (!string.IsNullOrWhiteSpace(trimmedLine) && sections.ContainsKey(currentSection))
                {
                    sections[currentSection].AppendLine(trimmedLine);
                }
            }

            return new MvpPlan
            {
                Id = Guid.NewGuid(),
                IdeaSessionId = ideaSessionId,
                Title = GetSectionContent(sections["title"], "AI-Generated MVP"),
                Goal = GetSectionContent(sections["problem"], "AI-generated goal"),
                ExperienceLevel = "Intermediate", // Default from AI generation
                ProblemStatement = GetSectionContent(sections["problem"], "Problem to be solved"),
                SolutionApproach = GetSectionContent(sections["solution"], "Solution approach"),
                ValueProposition = GetSectionContent(sections["value"], "Unique value proposition"),
                PrimaryTargetAudience = GetSectionContent(sections["primary"], "Primary target audience"),
                SecondaryTargetAudience = GetSectionContent(sections["secondary"], "Secondary audience"),
                KeyFeatures = GetSectionContent(sections["features"], "Core features"),
                TechnicalStack = GetSectionContent(sections["tech"], "Recommended tech stack"),
                EstimatedBudget = GetSectionContent(sections["budget"], "Budget estimate"),
                TimelineEstimate = GetSectionContent(sections["timeline"], "Timeline estimate"),
                MonetizationStrategy = GetSectionContent(sections["monetization"], "Monetization strategy"),
                NextSteps = GetSectionContent(sections["nextsteps"], "Next steps"),
                RawAiResponse = aiContent,
                // Legacy fields for backward compatibility
                Summary = GetSectionContent(sections["problem"], "AI-generated summary"),
                CreatedAt = DateTime.UtcNow
            };
        }

        private string GetSectionContent(StringBuilder section, string fallback)
        {
            var content = section.ToString().Trim();
            return string.IsNullOrEmpty(content) ? fallback : content;
        }
    }

    // Gemini API Response DTOs
    public class GeminiResponse
    {
        public GeminiCandidate[]? candidates { get; set; }
    }

    public class GeminiCandidate
    {
        public GeminiContent? content { get; set; }
    }

    public class GeminiContent
    {
        public GeminiPart[]? parts { get; set; }
    }

    public class GeminiPart
    {
        public string? text { get; set; }
    }
}
