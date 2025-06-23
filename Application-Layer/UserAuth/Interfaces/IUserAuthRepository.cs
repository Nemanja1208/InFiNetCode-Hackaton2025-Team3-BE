using Application_Layer.Common.Interfaces;
using Domain_Layer.Models;

namespace Application_Layer.UserAuth.Interfaces
{
    public interface IUserAuthRepository : IGenericRepository<UserModel>
    {
        Task<UserModel?> GetUserByProvider(string provider, string providerId);
    }
}
