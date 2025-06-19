using Microsoft.AspNetCore.Identity;
using System;

namespace Domain_Layer.Models
{
    public class UserModel : IdentityUser
    {
        // Add any additional properties for your user here
        // The user requested to remove UserName and RefreshTokenExpiryTime for now.
        // public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
