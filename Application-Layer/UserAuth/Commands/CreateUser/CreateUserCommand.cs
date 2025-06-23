using Application_Layer.UserAuth.Dtos;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.UserAuth.Commands.CreateUser
{
    public record CreateUserCommand(UserDataDto Dto) : IRequest<OperationResult<string>>;
}
