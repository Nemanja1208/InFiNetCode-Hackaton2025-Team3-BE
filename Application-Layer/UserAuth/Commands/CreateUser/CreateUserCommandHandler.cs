using Application_Layer.UserAuth.Interfaces;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.UserAuth.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<string>>
    {
        private readonly IUserAuthRepository _repository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserAuthRepository userAuthRepository, IMapper mapper)
        {
            _repository = userAuthRepository;
            _mapper = mapper;
        }
        public async Task<OperationResult<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserModel>(request.Dto);
            await _repository.CreateAsync(user);
            return OperationResult<string>.Success(user.Id);

        }
    }
}
