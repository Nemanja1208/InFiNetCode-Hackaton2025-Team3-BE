using Application_Layer.Steps.Commands.CreateStep;
using Application_Layer.Steps.Dtos;
using Application_Layer.Steps.Queries.GetStepsByIdeaSessionId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StepController(IMediator mediator) => _mediator = mediator;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStepRequestDto dto) =>
            await _mediator.Send(new CreateStepCommand(dto)) is var result && result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result);

        [Authorize]
        [HttpGet("by-ideasession/{ideaSessionId}")]
        public async Task<IActionResult> GetStepsByIdeaSessionId(Guid ideaSessionId) =>
            await _mediator.Send(new GetStepsByIdeaSessionIdQuery(ideaSessionId)) is var result && result.IsSuccess
                ? Ok(result)
                : NotFound(result);

    }
}
