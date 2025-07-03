using Application_Layer.StepTemplates.Dtos;
using Application_Layer.StepTemplates.Queries.GetAllStepTemplates;
using Application_Layer.StepTemplates.Queries.GetStepTemplateById;
using Application_Layer.StepTemplates.Commands.CreateStepTemplate;
using Application_Layer.StepTemplates.Commands.UpdateStepTemplate;
using Application_Layer.StepTemplates.Commands.SeedStepTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepTemplateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StepTemplateController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _mediator.Send(new GetAllStepTemplatesQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) =>
            await _mediator.Send(new GetStepTemplateByIdQuery(id)) is var result && result.IsSuccess
                ? Ok(result)
                : NotFound(result);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStepTemplateDto dto) =>
           await _mediator.Send(new CreateStepTemplateCommand(dto)) is var result && result.IsSuccess
               ? Ok(result)
               : BadRequest(result);

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StepTemplateDto dto) =>
            await _mediator.Send(new UpdateStepTemplateCommand(id, dto)) is var result && result.IsSuccess
                ? Ok(result)
                : BadRequest(result);

        [HttpPost("seed")] // New endpoint for seeding
        [AllowAnonymous] // Allow unauthenticated access for seeding during development
        public async Task<IActionResult> Seed()
        {
            var result = await _mediator.Send(new SeedStepTemplatesCommand());
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
