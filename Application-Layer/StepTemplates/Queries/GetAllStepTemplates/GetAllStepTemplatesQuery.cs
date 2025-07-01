using Application_Layer.StepTemplates.Dtos;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Queries.GetAllStepTemplates
{
    public record GetAllStepTemplatesQuery : IRequest<OperationResult<List<StepTemplateDto>>>;
}
