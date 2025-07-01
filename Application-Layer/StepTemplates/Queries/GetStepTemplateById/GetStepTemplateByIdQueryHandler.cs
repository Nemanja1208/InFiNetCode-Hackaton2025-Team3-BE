using Application_Layer.Common.Interfaces;
using Application_Layer.StepTemplates.Dtos;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Queries.GetStepTemplateById
{
    public class GetStepTemplateByIdQueryHandler : IRequestHandler<GetStepTemplateByIdQuery, OperationResult<StepTemplateDto>>
    {
        private readonly IGenericRepository<StepTemplate> _stepTemplateRepository;
        private readonly IMapper _mapper;

        public GetStepTemplateByIdQueryHandler(
            IGenericRepository<StepTemplate> stepTemplateRepository,
            IMapper mapper)
        {
            _stepTemplateRepository = stepTemplateRepository;
            _mapper = mapper;
        }
        public async Task<OperationResult<StepTemplateDto>> Handle(GetStepTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            var stepTemplate = await _stepTemplateRepository.GetByIdAsync(request.Id);
            if (stepTemplate is null)
            {
                return OperationResult<StepTemplateDto>.Failure($"Step template with Id {request.Id} not found.");
            }
            var stepTemplateDto = _mapper.Map<StepTemplateDto>(stepTemplate);
            return OperationResult<StepTemplateDto>.Success(stepTemplateDto);
        }
    }
}
