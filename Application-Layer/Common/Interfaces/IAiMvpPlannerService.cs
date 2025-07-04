using Domain_Layer.Models;

namespace Application_Layer.Common.Interfaces
{
    public interface IAiMvpPlannerService
    {
        Task<MvpPlan> GenerateMvpPlanAsync(IdeaSession ideaSession);
    }
}
