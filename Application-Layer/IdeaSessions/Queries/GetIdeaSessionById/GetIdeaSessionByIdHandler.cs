using Application_Layer.Common.Interfaces;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;

public class GetIdeaSessionByIdHandler : IRequestHandler<GetIdeaSessionByIdQuery, IdeaSessionWithStepsDto?>
{
    private readonly IGenericRepository<IdeaSession> _ideaRepository;
    private readonly IGenericRepository<Step> _stepRepository;
    private readonly IMapper _mapper;

    public GetIdeaSessionByIdHandler(
        IGenericRepository<IdeaSession> ideaRepository,
        IGenericRepository<Step> stepRepository,
        IMapper mapper)
    {
        _ideaRepository = ideaRepository;
        _stepRepository = stepRepository;
        _mapper = mapper;
    }

    public async Task<IdeaSessionWithStepsDto?> Handle(GetIdeaSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var idea = await _ideaRepository.GetByIdAsync(request.IdeaId);

        if (idea == null || idea.UserId != request.UserId)
            return null;

        // Hämta steps separat eftersom Include() inte finns
        var steps = await _stepRepository.GetAllAsync();
        var matchingSteps = steps
            .Where(s => s.IdeaSessionId == idea.Id)
            .OrderBy(s => s.StepOrder)
            .ToList();

        idea.Steps = matchingSteps;

        return _mapper.Map<IdeaSessionWithStepsDto>(idea);
    }
}
