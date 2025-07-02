using Application_Layer.StepTemplates.Dtos;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Commands.UpdateStepTemplate
{
    public record UpdateStepTemplateCommand(Guid Id, StepTemplateDto Dto) : IRequest<OperationResult<StepTemplateDto>>;

}
