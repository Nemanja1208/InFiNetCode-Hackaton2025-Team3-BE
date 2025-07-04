using Application_Layer.Common.Interfaces;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application_Layer.IdeaSessions.Queries.GetIdeaSessionWithPlans
{
    public class GetIdeaSessionWithPlansQueryHandler : IRequestHandler<GetIdeaSessionWithPlansQuery, IdeaSessionWithStepsDto?>
    {
        private readonly IGenericRepository<IdeaSession> _repository;
        private readonly IMapper _mapper;

        public GetIdeaSessionWithPlansQueryHandler(IGenericRepository<IdeaSession> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IdeaSessionWithStepsDto?> Handle(GetIdeaSessionWithPlansQuery request, CancellationToken cancellationToken)
        {
            // Use the existing repository method to include related entities
            var session = await _repository.GetByIdAsync(request.IdeaSessionId, s => s.Steps, s => s.MvpPlans);

            // The repository should handle the user check, but we can double-check here if needed.
            if (session == null || session.UserId != request.UserId)
            {
                return null;
            }

            return _mapper.Map<IdeaSessionWithStepsDto>(session);
        }
    }
}
