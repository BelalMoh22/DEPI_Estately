//using Microsoft.AspNetCore.Identity;
//using Estately.Infrastructure.Data;
//using Estately.Core.Entities;

//public static class DefaultUsersSeeder
//{
//    public static async Task SeedAsync(
//        UserManager<ApplicationUser> userManager)
//    {
//        await SeedEmployees(userManager);
//        await SeedDevelopers(userManager);
//        await SeedAdmins(userManager);
//    }

//    private static async Task SeedEmployees(UserManager<ApplicationUser> userManager)
//    {
//        var employees = new List<(string Email, string UserName, string Password)>
//        {
//            ("sales@estately.com","ahmed.hassan","Manager1"),
//            ("marketing@estately.com","sara.mostafa","Manager2"),
//            ("support@estately.com","omar.khaled","Manager3"),
//            ("pm@estately.com","yara.mahmoud","Manager4"),
//            ("legal@estately.com","mostafa.adel","Manager5"),
//            ("ahmed.ali@estately.com","ahmed.ali","Manager7"),
//            ("sara.fathy@estately.com","sara.fathy","Manager8"),
//            ("mahmoud.adel@estately.com","mahmoud.adel","Manager9"),

//            ("hussein@estately.com","hussein.magdy","Employee1"),
//            ("nada@estately.com","nada.tarek","Employee2"),
//            ("ali.samir@estately.com","ali.samir","Employee3"),
//            ("mina.adel@estately.com","mina.adel","Employee4"),
//            ("farida@estately.com","farida.maher","Employee5"),
//            ("karim@estately.com","karim.mohamed","Employee6"),
//            ("hanaa@estately.com","hanaa.youssef","Employee7"),
//            ("ehab@estately.com","ehab.mostafa","Employee8"),
//            ("rana@estately.com","rana.samer","Employee9"),
//            ("youssef@estately.com","youssef.eid","Employee10"),
//            ("nourhan@estately.com","nourhan.adel","Employee11"),
//            ("mohamed.adel@estately.com","mohamed.adel2","Employee12"),
//            ("hadi.mansour@estately.com","hadi.mansour","Employee13"),
//            ("malak.samir@estately.com","malak.samir","Employee14"),
//            ("tarek.hamed@estately.com","tarek.hamed","Employee15"),
//            ("salma.mansy@estately.com","salma.mansy","Employee16"),
//            ("riah.kareem@estately.com","riah.kareem","Employee17"),
//            ("fady.nasser@estately.com","fady.nasser","Employee18"),
//            ("hana.galal@estately.com","hana.galal","Employee19"),
//            ("mario.sameh@estately.com","mario.sameh","Employee20")
//        };

//        foreach (var emp in employees)
//        {
//            if (await userManager.FindByEmailAsync(emp.Email) == null)
//            {
//                var user = new ApplicationUser
//                {
//                    Email = emp.Email,
//                    UserName = emp.UserName,
//                    UserTypeID = 2,
//                    EmailConfirmed = true
//                };

//                await userManager.CreateAsync(user, emp.Password);
//            }
//        }
//    }

//    private static async Task SeedDevelopers(UserManager<ApplicationUser> userManager)
//    {
//        var developers = new List<(string Email, string UserName, string Password)>
//        {
//            ("mariam.saad@mountainview.com","mv.mariam","Developer1"),
//            ("fady.nader@sodic.com","sodic.fady","Developer2"),
//            ("reem.khaled@palmhills.com","palmhills.reem","Developer3"),
//            ("youssef.maged@orascomdev.com","orascom.youssef","Developer4"),
//            ("omar.helmy@tatweermisr.com","tatweer.omar","Developer5"),
//            ("rana.samir@emaarmisr.com","emaar.rana","Developer6"),
//            ("tarek.nabil@ifg.com","ifg.tarek","Developer7"),
//            ("hala.adel@misritalia.com","mi.hala","Developer8"),
//            ("youssef.rami@alahlysabbour.com","sabbour.youssef","Developer9"),
//            ("walid.tarek@tmg.com","tmg.walid","Developer10")
//        };

//        foreach (var dev in developers)
//        {
//            if (await userManager.FindByEmailAsync(dev.Email) == null)
//            {
//                var user = new ApplicationUser
//                {
//                    Email = dev.Email,
//                    UserName = dev.UserName,
//                    UserTypeID = 3,
//                    EmailConfirmed = true
//                };

//                await userManager.CreateAsync(user, dev.Password);
//            }
//        }
//    }
//    private static async Task SeedAdmins(UserManager<ApplicationUser> userManager)
//    {
//        var Admins = new List<(string Email, string UserName, string Password)>
//        {
//            ("admin@estately.com","Admin","Admin@1234"),
//        };

//        foreach (var dev in Admins)
//        {
//            if (await userManager.FindByEmailAsync(dev.Email) == null)
//            {
//                var user = new ApplicationUser
//                {
//                    Email = dev.Email,
//                    UserName = dev.UserName,
//                    UserTypeID = 4,
//                    EmailConfirmed = true
//                };

//                await userManager.CreateAsync(user, dev.Password);
//            }
//        }
//    }
//}