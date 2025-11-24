//using Microsoft.AspNetCore.Identity;
//using Estately.Infrastructure.Data;
//using Estately.Core.Entities;

//namespace Estately.Infrastructure.Data
//{
//    public static class DefaultUsersSeeder
//    {
//        public static async Task SeedAsync(
//            UserManager<ApplicationUser> userManager)
//        {
//            await SeedAdmins(userManager);
//        }

//        private static async Task SeedAdmins(UserManager<ApplicationUser> userManager)
//        {
//            var employees = new List<(string Email, string UserName, string Password)>
//    {
//        ("admin@estately.com", "Admin", "Admin@1234"),
//    };

//            foreach (var emp in employees)
//            {
//                if (await userManager.FindByEmailAsync(emp.Email) == null)
//                {
//                    var user = new ApplicationUser
//                    {
//                        Email = emp.Email,
//                        UserName = emp.UserName,     // safer
//                        UserTypeID = 4,
//                        EmailConfirmed = true
//                    };

//                    var result = await userManager.CreateAsync(user, emp.Password);

//                    if (!result.Succeeded)
//                    {
//                        foreach (var error in result.Errors)
//                            Console.WriteLine("❌ Admin seeding error: " + error.Description);
//                    }
//                }
//            }
//        }

//    }
//}
