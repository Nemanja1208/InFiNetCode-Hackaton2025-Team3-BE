using Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;
using Application_Layer.IdeaSessions.Commands.UpdateIdeaSessionTitle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application_Layer.IdeaSessions.Commands.CreateIdeaSession;
using Application_Layer.IdeaSessions.Commands.GenerateMvpRecommendations; // Add this
using Application_Layer.IdeaSessions.DTOs;
using Application_Layer.Steps.Dtos;
using System.Security.Claims;

namespace API_Layer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IdeaSessionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdeaSessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetIdeaSessionByIdQuery(id, userId));

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("CreateIdeaSession")]
    public async Task<IActionResult> CreateIdeaSession([FromBody] CreateIdeaSessionDto request)
    {
        // Get UserId from authenticated user (JWT claims)
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID in token.");
        }

        // Create the command with the DTO and UserId
        var command = new CreateIdeaSessionCommand(request, userId);

        // Send to MediatR and handle OperationResult
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            // Return 201 Created, with route-value corresponding to DTO's Id field
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.Id }, // Use result.Data.Id as IdeaSessionDto now has 'Id'
                result.Data
            );
        }
        else
        {
            // Return 400 Bad Request for validation errors or 500 for other failures
            return BadRequest(result.Errors); // Or StatusCode(500, result.Errors) for server-side issues
        }
    }

    [HttpPost("{id}/generate-recommendations")]
    public async Task<IActionResult> GenerateMvpRecommendations(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID in token.");
        }

        var command = new GenerateMvpRecommendationsCommand(id, userId);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTitle(Guid id, [FromBody] UpdateIdeaSessionTitleCommand command)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        if (id != command.IdeaSessionId)
            return BadRequest("Route ID and body ID do not match.");

        command.UserId = userId;

        var result = await _mediator.Send(command);

        if (result is null)
            return NotFound();

        return Ok(new
        {
            ideaId = result.Id,
            title = result.Title,
            status = result.Status,
            createdAt = result.CreatedAt
        });
    }

    [HttpGet("{id}/plan")]
    public async Task<IActionResult> GetPlan(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        // Use the new, specific query to get the session with its plans
        var ideaSessionResult = await _mediator.Send(new Application_Layer.IdeaSessions.Queries.GetIdeaSessionWithPlans.GetIdeaSessionWithPlansQuery(id, userId));

        if (ideaSessionResult?.MvpPlans == null || !ideaSessionResult.MvpPlans.Any())
        {
            return NotFound("No plan has been generated for this session yet.");
        }

        // Return the most recently created plan
        var latestPlan = ideaSessionResult.MvpPlans.OrderByDescending(p => p.CreatedAt).First();
        return Ok(latestPlan);
    }

    [HttpGet("{id}/next-question")]
    public async Task<IActionResult> GetNextQuestion(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        // Get all steps for this IdeaSession
        var stepsQuery = new Application_Layer.Steps.Queries.GetStepsByIdeaSessionId.GetStepsByIdeaSessionIdQuery(id);
        var stepsResult = await _mediator.Send(stepsQuery);

        if (!stepsResult.IsSuccess)
            return BadRequest(stepsResult.Errors);

        // Debug: Log all steps to see what we're getting
        var allSteps = stepsResult.Data ?? new List<StepResponseDto>();

        // Find the first step without UserInput (ordered by Order)
        var nextStep = allSteps
            .Where(s => string.IsNullOrWhiteSpace(s.UserInput))
            .OrderBy(s => s.Order)
            .FirstOrDefault();

        if (nextStep == null)
            return Ok(new
            {
                message = "All questions have been answered",
                allCompleted = true,
                debug_totalSteps = allSteps.Count,
                debug_answeredSteps = allSteps.Count(s => !string.IsNullOrWhiteSpace(s.UserInput))
            });

        // Get the StepTemplate to include the actual question
        var stepTemplateQuery = new Application_Layer.StepTemplates.Queries.GetStepTemplateById.GetStepTemplateByIdQuery(nextStep.StepTemplateId);
        var stepTemplateResult = await _mediator.Send(stepTemplateQuery);

        var question = stepTemplateResult?.Data?.Question ?? "Question not found";
        var title = stepTemplateResult?.Data?.Title ?? "Unknown";

        return Ok(new
        {
            stepId = nextStep.Id,
            stepTemplateId = nextStep.StepTemplateId,
            order = nextStep.Order,
            title = title,
            question = question,
            allCompleted = false,
            debug_totalSteps = allSteps.Count,
            debug_answeredSteps = allSteps.Count(s => !string.IsNullOrWhiteSpace(s.UserInput)),
            debug_nextStepUserInput = nextStep.UserInput ?? "NULL"
        });
    }

    [HttpGet("{id}/progress")]
    public async Task<IActionResult> GetProgress(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        // Get all steps for this IdeaSession
        var stepsQuery = new Application_Layer.Steps.Queries.GetStepsByIdeaSessionId.GetStepsByIdeaSessionIdQuery(id);
        var stepsResult = await _mediator.Send(stepsQuery);

        if (!stepsResult.IsSuccess)
            return BadRequest(stepsResult.Errors);

        var steps = stepsResult.Data ?? new List<StepResponseDto>();
        var totalQuestions = steps.Count;
        var answeredQuestions = steps.Count(s => !string.IsNullOrWhiteSpace(s.UserInput));

        return Ok(new
        {
            totalQuestions,
            answeredQuestions,
            remainingQuestions = totalQuestions - answeredQuestions,
            progressPercentage = totalQuestions > 0 ? (answeredQuestions * 100) / totalQuestions : 0,
            isCompleted = answeredQuestions == totalQuestions && totalQuestions > 0
        });
    }
}
