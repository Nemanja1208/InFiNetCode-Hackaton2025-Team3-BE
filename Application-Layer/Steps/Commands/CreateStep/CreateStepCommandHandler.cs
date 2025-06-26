using Application_Layer.Common.Interfaces;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.Steps.Commands.CreateStep
{
    public class CreateStepCommandHandler : IRequestHandler<CreateStepCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Step> _repository;
        private readonly IMapper _mapper;

        public CreateStepCommandHandler(IGenericRepository<Step> repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<string>> Handle(CreateStepCommand request, CancellationToken cancellationToken)
        {
            var step = _mapper.Map<Step>(request.Dto);
            step.Id = Guid.NewGuid(); 
            await _repository.CreateAsync(step);
            return OperationResult<string>.Success(step.Id.ToString());
        }
    }
}
