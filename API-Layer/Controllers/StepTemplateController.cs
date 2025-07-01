using Application_Layer.StepTemplates.Commands;
using Application_Layer.StepTemplates.Dtos;
using Application_Layer.StepTemplates.Queries.GetAllStepTemplates;
using Application_Layer.StepTemplates.Queries.GetStepTemplateById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    }
}
