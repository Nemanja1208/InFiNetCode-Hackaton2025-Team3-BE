using Application_Layer.Steps.Commands.CreateStep;
using Application_Layer.Steps.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Layer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StepController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StepController(IMediator mediator) => _mediator = mediator;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStepDto dto) =>
            await _mediator.Send(new CreateStepCommand(dto)) is var result && result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
    }
}
