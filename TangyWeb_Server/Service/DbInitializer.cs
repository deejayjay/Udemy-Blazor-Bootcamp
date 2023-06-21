using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tangy_Common;
using Tangy_DataAccess.Data;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                // Check if there are pending migrations and if any exist, apply them.
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

                // Check if the admin role exists. If not, creates both the roles.
                // Runs only once.
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    // Another way of calling an async method (useful if the parent method is async)
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                }
                else
                {
                    return;
                }

                IdentityUser user = new()
                {
                    UserName = "deepak@dotnetmastery.com",
                    Email = "deepak@dotnetmastery.com",
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, "Password@1234").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
