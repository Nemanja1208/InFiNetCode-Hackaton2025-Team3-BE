using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application_Layer.Common.Interfaces;
using Application_Layer.IdeaSessions.DTOs;
using Application_Layer.Features.IdeaSessions.Queries.GetByUser;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application_Layer.IdeaSessions.Features.IdeaSessions.Queries.GetByUser;

namespace Application_Layer.Features.IdeaSessions.Queries.GetByUser
{
    public class GetIdeaSessionsByUserQueryHandler
        : IRequestHandler<GetIdeaSessionsByUserQuery, IEnumerable<IdeaSessionWithStepsDto>>
    {
        private readonly IGenericRepository<IdeaSession> _repo;

        private readonly IMapper _mapper;

        public GetIdeaSessionsByUserQueryHandler(
            IGenericRepository<IdeaSession> repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IdeaSessionWithStepsDto>> Handle(
            GetIdeaSessionsByUserQuery request,
            CancellationToken cancellationToken)
        {
            // Hämta alla och filtrera i minnet
            var allSessions = await _repo.GetAllAsync();
            var filtered = allSessions
                // TODO: 'ICurrentUserService.UserId' is inaccessible due to its protection level. Replace with correct access method.
                .Where(s => s.UserId == Guid.Empty) // Placeholder until correct UserId access is implemented
                .OrderByDescending(s => s.CreatedAt);

            return filtered
                .Select(s => _mapper.Map<IdeaSessionWithStepsDto>(s));
        }
    }
}