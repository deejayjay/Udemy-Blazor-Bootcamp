using Microsoft.AspNetCore.Identity;

#nullable disable
namespace Tangy_DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
