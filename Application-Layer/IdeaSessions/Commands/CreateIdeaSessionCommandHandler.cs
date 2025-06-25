using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using MediatR;
using Application_Layer.IdeaSessions.Dto;
using Domain_Layer.Models;
using Infrastructure_Layer.Repositories; 

namespace Application_Layer.IdeaSessions.Commands
{
    public class CreateIdeaSessionCommandHandler : IRequestHandler<CreateIdeaSessionCommand, IdeaSessionDto>
    {
        private readonly IGenericRepository<IdeaSession> _repo;
        private readonly IMapper _mapper;

        public CreateIdeaSessionCommandHandler(IGenericRepository<IdeaSession> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IdeaSessionDto> Handle(CreateIdeaSessionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<IdeaSession>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            await _repo.CreateAsync(entity);

            return _mapper.Map<IdeaSessionDto>(entity);
        }
    }
}

