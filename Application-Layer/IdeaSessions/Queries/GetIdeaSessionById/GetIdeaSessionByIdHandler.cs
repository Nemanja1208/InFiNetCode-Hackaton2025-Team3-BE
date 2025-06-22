using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using Infrastructure_Layer.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;

public class GetIdeaSessionByIdHandler : IRequestHandler<GetIdeaSessionByIdQuery, IdeaSessionWithStepsDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetIdeaSessionByIdHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IdeaSessionWithStepsDto?> Handle(GetIdeaSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var idea = await _context.IdeaSessions
            .Include(i => i.Steps.OrderBy(s => s.StepOrder))
            .FirstOrDefaultAsync(i => i.Id == request.IdeaId && i.UserId == request.UserId, cancellationToken);

        return idea == null ? null : _mapper.Map<IdeaSessionWithStepsDto>(idea);
    }
}
