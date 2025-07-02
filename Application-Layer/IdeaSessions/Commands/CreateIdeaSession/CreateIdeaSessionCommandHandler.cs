using Application_Layer.Common.Interfaces;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Domain_Layer.Models;
using MediatR;

namespace Application_Layer.IdeaSessions.Commands.CreateIdeaSession
{
    public class CreateIdeaSessionCommandHandler : IRequestHandler<CreateIdeaSessionCommand, OperationResult<IdeaSessionDto>>
    {
        private readonly IGenericRepository<IdeaSession> _repo;
        private readonly IMapper _mapper;

        public CreateIdeaSessionCommandHandler(IGenericRepository<IdeaSession> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationResult<IdeaSessionDto>> Handle(CreateIdeaSessionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1) Map DTO from command to entity
                var ideaSession = _mapper.Map<IdeaSession>(request.Dto);

                // Set generated properties
                ideaSession.Id = Guid.NewGuid();
                ideaSession.CreatedAt = DateTime.UtcNow;
                // UserId is obtained from the authenticated user and passed directly to the command.
                ideaSession.UserId = request.UserId;

                // 2) Save to database
                await _repo.CreateAsync(ideaSession);

                // 3) Return success with mapped DTO
                var ideaSessionDto = _mapper.Map<IdeaSessionDto>(ideaSession);
                return OperationResult<IdeaSessionDto>.Success(ideaSessionDto);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return OperationResult<IdeaSessionDto>.Failure($"Failed to create idea session: {ex.Message}");
            }
        }
    }
}
