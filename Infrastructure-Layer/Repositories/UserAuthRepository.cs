using Application_Layer.UserAuth.Interfaces;
using Domain_Layer.Models;
using Infrastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Layer.Repositories
{
    public class UserAuthRepository : GenericRepository<UserModel>, IUserAuthRepository
    {
        public UserAuthRepository(ApplicationDbContext context) : base(context) 
        {

        }

        public async Task<UserModel?> GetUserByProvider(string provider, string providerId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Provider == provider && u.ProviderId == providerId);
        }
    }
}
