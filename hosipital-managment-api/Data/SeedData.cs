using Microsoft.AspNetCore.Identity;

namespace hosipital_managment_api.Data
{
    public static class SeedData
    {
        
        public static void SeedAdmin(RoleManager<IdentityRole> roleManager, UserManager<ApiUser> userManager,string password)
        {
            if(userManager.FindByNameAsync("Admin").Result == null)
            {
                var admin = new ApiUser()
                {
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "12345789",
                    FirstName = "Admin",
                    LastName = "Admin"
                };
                var result = userManager.CreateAsync(admin, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Doctor", "Pharmacist", "Patient", "Nurse", "Laboratorist" };
            foreach(string role in roles){
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    var newRole = new IdentityRole
                    {
                        Name = role,
                    };
                    roleManager.CreateAsync(newRole).Wait();
                }
            }
        }

    }
}
