using Application_Layer.Common.Interfaces;
using Domain_Layer.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application_Layer.IdeaSessions.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace Application_Layer.IdeaSessions.Commands.GenerateMvpRecommendations
{
    public class GenerateMvpRecommendationsCommandHandler : IRequestHandler<GenerateMvpRecommendationsCommand, OperationResult<IdeaSessionDto>>
    {
        private readonly IGenericRepository<IdeaSession> _ideaSessionRepository;
        private readonly IGenericRepository<MvpPlan> _mvpPlanRepository;
        private readonly IAiMvpPlannerService _aiMvpPlannerService;
        private readonly IMapper _mapper;

        public GenerateMvpRecommendationsCommandHandler(
            IGenericRepository<IdeaSession> ideaSessionRepository,
            IGenericRepository<MvpPlan> mvpPlanRepository,
            IAiMvpPlannerService aiMvpPlannerService,
            IMapper mapper)
        {
            _ideaSessionRepository = ideaSessionRepository;
            _mvpPlanRepository = mvpPlanRepository;
            _aiMvpPlannerService = aiMvpPlannerService;
            _mapper = mapper;
        }

        public async Task<OperationResult<IdeaSessionDto>> Handle(GenerateMvpRecommendationsCommand request, CancellationToken cancellationToken)
        {
            // Load IdeaSession with all related data in a single query
            var ideaSession = await _ideaSessionRepository.GetByIdAsync(
                request.IdeaSessionId,
                query => query
                    .Include(i => i.Steps)
                    .ThenInclude(s => s.StepTemplate)
            );

            if (ideaSession == null || ideaSession.UserId != request.UserId)
            {
                return OperationResult<IdeaSessionDto>.Failure("Idea session not found or user does not have access.");
            }

            // Validate that user has answered all required questions
            if (ideaSession.Steps == null || ideaSession.Steps.Count < 5) // Require at least 5 answers for meaningful AI generation
            {
                return OperationResult<IdeaSessionDto>.Failure($"Please answer more questions before generating MVP recommendations. You have answered {ideaSession.Steps?.Count ?? 0} questions. At least 5 answers are required for quality AI recommendations.");
            }

            var mvpPlan = await _aiMvpPlannerService.GenerateMvpPlanAsync(ideaSession);

            // Create the MvpPlan directly in the database to avoid concurrency issues
            await _mvpPlanRepository.CreateAsync(mvpPlan);

            // Reload the IdeaSession with the new MvpPlan to return updated data
            var updatedIdeaSession = await _ideaSessionRepository.GetByIdAsync(
                request.IdeaSessionId,
                query => query
                    .Include(i => i.Steps)
                    .ThenInclude(s => s.StepTemplate)
                    .Include(i => i.MvpPlans)
            );

            var ideaSessionDto = _mapper.Map<IdeaSessionDto>(updatedIdeaSession);
            return OperationResult<IdeaSessionDto>.Success(ideaSessionDto);
        }
    }
}
