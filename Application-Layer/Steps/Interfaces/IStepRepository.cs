using Application_Layer.Common.Interfaces;
using Domain_Layer.Models;

namespace Application_Layer.Steps.Interfaces
{
    public interface IStepRepository : IGenericRepository<Step>
    {
        Task<IEnumerable<Step>> GetStepsByIdeaSessionId(Guid ideaSessionId);
    }
}
