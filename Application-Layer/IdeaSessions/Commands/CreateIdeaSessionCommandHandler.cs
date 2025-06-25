using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using MediatR;
using Application_Layer.IdeaSessions.Dto;
using Domain_Layer.Models;
using Application_Layer.Common.Interfaces;
using Application_Layer.IdeaSessions.Commands;
using Application_Layer.Common.Mappings;



namespace Application_Layer.IdeaSessions.Commands
{
    public class CreateIdeaSessionCommandHandler : IRequestHandler<CreateIdeaSessionCommand, IdeaSessionDto>
    {
        private readonly IGenericRepository<IdeaSession> _repo;
        private readonly IMapper _mapper;

        // Konstruktorn DI-injicerar repository och mapper
        public CreateIdeaSessionCommandHandler(
            IGenericRepository<IdeaSession> repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IdeaSessionDto> Handle(CreateIdeaSessionCommand request, CancellationToken cancellationToken)
        {
            // Mappa command → entity
            var entity = _mapper.Map<IdeaSession>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            // Skapa i databasen med det interface du har
            await _repo.CreateAsync(entity);

            // Returnera DTO
            return _mapper.Map<IdeaSessionDto>(entity);
        }
    }
}
