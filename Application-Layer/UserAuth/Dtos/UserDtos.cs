
namespace Application_Layer.UserAuth.Dtos
{
    public class UserDataDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;    // "Google" or "GitHub"
        public string ProviderId { get; set; } = string.Empty;  // the external user ID
    }
}
