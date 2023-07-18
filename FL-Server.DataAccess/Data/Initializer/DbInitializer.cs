using FL_Server.Data;
using FL_Server.Models;
using FL_Server.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FL_Server.DataAccess.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {


        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(SD.AdminUser).GetAwaiter().GetResult())

            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminUser)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EndUser)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.DataPartner)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.DataScientist)).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "FL4EServer@admin.com",
                    Email = "FL4EServer@admin.com",
                    Affilation = "FL4E",
                    EmailConfirmed = true,
                    FirstName = "Ashkan",
                    LastName = "Pirmani",
                    UserRole = 2,
                    IsActivated = true

                }, "m#EB%4gmy%").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUser.FirstOrDefault(u => u.Email == "FL4EServer@admin.com");

                _userManager.AddToRoleAsync(user, SD.AdminUser).GetAwaiter().GetResult();

            }
            return;

        }

    }
}
