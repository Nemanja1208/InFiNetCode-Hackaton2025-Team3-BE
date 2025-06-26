using Application_Layer.Steps.Dtos;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.Steps.Commands.CreateStep
{
    public record CreateStepCommand(CreateStepDto Dto) : IRequest<OperationResult<string>>;

}
