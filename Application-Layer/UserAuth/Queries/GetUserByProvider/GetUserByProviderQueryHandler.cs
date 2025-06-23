using Application_Layer.UserAuth.Dtos;
using Application_Layer.UserAuth.Interfaces;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.UserAuth.Queries.GetUserByProvider
{
    public class GetUserByProviderQueryHandler : IRequestHandler<GetUserByProviderQuery, OperationResult<UserDataDto>>
    {
        private readonly IUserAuthRepository _repository;
        private readonly IMapper _mapper;

        public GetUserByProviderQueryHandler(IUserAuthRepository userAuthRepository, IMapper mapper)
        {
            _repository = userAuthRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDataDto>> Handle(GetUserByProviderQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByProvider(request.Provider, request.ProviderId);
            
            return user is null ? 
                OperationResult<UserDataDto>.Failure("User not found.") :
                OperationResult<UserDataDto>.Success(_mapper.Map<UserDataDto>(user));
        }
    }
}
