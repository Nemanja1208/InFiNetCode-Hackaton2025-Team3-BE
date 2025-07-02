using Application_Layer.Common.Interfaces;
using Application_Layer.StepTemplates.Dtos;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Commands.UpdateStepTemplate
{
    public class UpdateStepTemplateCommandHandler : IRequestHandler<UpdateStepTemplateCommand, OperationResult<StepTemplateDto>>
    {
        private readonly IGenericRepository<StepTemplate> _repository;
        private readonly IMapper _mapper;

        public UpdateStepTemplateCommandHandler(IGenericRepository<StepTemplate> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<StepTemplateDto>> Handle(UpdateStepTemplateCommand request, CancellationToken cancellationToken)
        {
            var existingTemplate = await _repository.GetByIdAsync(request.Id);
            if (existingTemplate is null)
                return OperationResult<StepTemplateDto>.Failure($"Step template with Id {request.Id} not found");

            _mapper.Map(request.Dto, existingTemplate);

            await _repository.UpdateAsync(existingTemplate);

            return OperationResult<StepTemplateDto>.Success(_mapper.Map<StepTemplateDto>(existingTemplate));
        }
    }
}
