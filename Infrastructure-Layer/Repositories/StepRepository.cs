using Application_Layer.Steps.Interfaces;
using Domain_Layer.Models;
using Infrastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Layer.Repositories
{
    public class StepRepository : GenericRepository<Step>, IStepRepository
    {
        public StepRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Step>> GetStepsByIdeaSessionId(Guid ideaSessionId)
        {
            return await _dbSet
                .Where(s => s.IdeaSessionId == ideaSessionId)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }
    }
}