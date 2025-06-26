using Application_Layer.Common.Interfaces;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.IdeaSessions.Commands.UpdateIdeaSessionTitle;

public class UpdateIdeaSessionTitleHandler : IRequestHandler<UpdateIdeaSessionTitleCommand, IdeaSession?>
{
    private readonly IGenericRepository<IdeaSession> _ideaRepository;

    public UpdateIdeaSessionTitleHandler(IGenericRepository<IdeaSession> ideaRepository)
    {
        _ideaRepository = ideaRepository;
    }

    public async Task<IdeaSession?> Handle(UpdateIdeaSessionTitleCommand request, CancellationToken cancellationToken)
    {
        var idea = await _ideaRepository.GetByIdAsync(request.IdeaSessionId);

        if (idea is null || idea.UserId != request.UserId)
            return null;

        idea.Title = request.Title;
        idea.UpdatedAt = DateTime.UtcNow;

        await _ideaRepository.UpdateAsync(idea);
        return idea;
    }
}
