using Application_Layer.Common.Interfaces;
using Application_Layer.StepTemplates.Dtos;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Commands
{
    public class CreateStepTemplateCommandHandler : IRequestHandler<CreateStepTemplateCommand, OperationResult<StepTemplateDto>>
    {
        private readonly IGenericRepository<StepTemplate> _repository;
        private readonly IMapper _mapper;

        public CreateStepTemplateCommandHandler(IGenericRepository<StepTemplate> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<StepTemplateDto>> Handle(CreateStepTemplateCommand request, CancellationToken cancellationToken)
        {
            var stepTemplate = _mapper.Map<StepTemplate>(request.Dto);
            stepTemplate.Id = Guid.NewGuid();
            await _repository.CreateAsync(stepTemplate);
            var savedStepTemplate = await _repository.GetByIdAsync(stepTemplate.Id);
            return savedStepTemplate is null
                ? OperationResult<StepTemplateDto>.Failure("Step template wasn't successfully saved in database.")
                : OperationResult<StepTemplateDto>.Success(_mapper.Map<StepTemplateDto>(savedStepTemplate));
        }
    }
}
