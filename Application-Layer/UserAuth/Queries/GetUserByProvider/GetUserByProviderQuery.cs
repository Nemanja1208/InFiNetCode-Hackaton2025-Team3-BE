using Application_Layer.UserAuth.Dtos;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.UserAuth.Queries.GetUserByProvider
{
    public record GetUserByProviderQuery(string Provider, string ProviderId) : IRequest<OperationResult<UserDataDto>>;
}
