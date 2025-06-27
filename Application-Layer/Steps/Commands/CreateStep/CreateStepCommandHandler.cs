using Application_Layer.Common.Interfaces;
using Application_Layer.Steps.Dtos;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.Steps.Commands.CreateStep
{
    public class CreateStepCommandHandler : IRequestHandler<CreateStepCommand, OperationResult<CreateStepResponseDto>>
    {
        private readonly IGenericRepository<Step> _repository;
        private readonly IMapper _mapper;

        public CreateStepCommandHandler(IGenericRepository<Step> repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<CreateStepResponseDto>> Handle(CreateStepCommand request, CancellationToken cancellationToken)
        {
            var step = _mapper.Map<Step>(request.Dto);
            step.Id = Guid.NewGuid(); 

            await _repository.CreateAsync(step);
            var savedStep = await _repository.GetByIdAsync(step.Id);
            return savedStep is null ?
                OperationResult<CreateStepResponseDto>.Failure("Step could not be found after creation.") :
                OperationResult<CreateStepResponseDto>.Success(_mapper.Map<CreateStepResponseDto>(savedStep));
        }
    }
}
