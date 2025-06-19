using Domain_Layer.Models;
using System.Threading.Tasks;

namespace Application_Layer.Jwt
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(UserModel user);
    }
}
