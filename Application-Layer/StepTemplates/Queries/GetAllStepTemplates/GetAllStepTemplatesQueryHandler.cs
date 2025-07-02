using Application_Layer.Common.Interfaces;
using Application_Layer.StepTemplates.Dtos;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.StepTemplates.Queries.GetAllStepTemplates
{
    public class GetAllStepTemplatesQueryHandler : IRequestHandler<GetAllStepTemplatesQuery, OperationResult<List<StepTemplateDto>>>
    {
        private readonly IGenericRepository<StepTemplate> _stepTemplateRepository;
        private readonly IMapper _mapper;

        public GetAllStepTemplatesQueryHandler(
            IGenericRepository<StepTemplate> stepTemplateRepository,
            IMapper mapper)
        {
            _stepTemplateRepository = stepTemplateRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<StepTemplateDto>>> Handle(GetAllStepTemplatesQuery request, CancellationToken cancellationToken)
        {
            var stepTemplates = await _stepTemplateRepository.GetAllAsync();

            var stepTemplateDtos = _mapper.Map<List<StepTemplateDto>>(stepTemplates)
                                          .OrderBy(dto => dto.Order)
                                          .ToList();

            return OperationResult<List<StepTemplateDto>>.Success(stepTemplateDtos);
        }
    }
}
