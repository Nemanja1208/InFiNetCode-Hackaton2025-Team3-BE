using Application_Layer.StepTemplates.Dtos;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Queries.GetStepTemplateById
{
    public record GetStepTemplateByIdQuery(Guid Id) : IRequest<OperationResult<StepTemplateDto>>;

}
