using Application_Layer.StepTemplates.Dtos;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Commands.CreateStepTemplate
{
    public record CreateStepTemplateCommand(CreateStepTemplateDto Dto) : IRequest<OperationResult<StepTemplateDto>>;
}
