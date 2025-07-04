using MediatR;
using Domain_Layer.Models;

namespace Application_Layer.StepTemplates.Commands.SeedStepTemplates
{
    public record SeedStepTemplatesCommand() : IRequest<OperationResult<string>>;
}
