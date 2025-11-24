using Estately.Core.Entities.Identity;
using Estately.Services.Interfaces.Email;
using Estately.Services.Implementations;
using Microsoft.AspNetCore.Identity;

namespace Estately.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(connectionString));

            // Use custom ApplicationUser/ApplicationRole with int keys
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                options => options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = false,
                    RequiredLength = 6,
                    RequiredUniqueChars = 1
                }
                )
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();


            //builder.Services.AddControllersWithViews();
            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //Add Context with Connection 
            //builder.Services.AddDbContext<AppDBContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // register repositories & unitofwork
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceProperty, ServiceProperty>();
            builder.Services.AddScoped<IServiceZone, ServiceZone>();
            builder.Services.AddScoped<IServiceUserType, ServiceUserType>();
            builder.Services.AddScoped<IServiceDepartment, ServiceDepartment>();
            builder.Services.AddScoped<IServiceBranch, ServiceBranch>();
            builder.Services.AddScoped<IServiceEmployee, ServiceEmployee>();
            builder.Services.AddScoped<IServicePropertyType, ServicePropertyType>();
            builder.Services.AddScoped<IServiceAppointment, ServiceAppointment>();
            builder.Services.AddScoped<IServicePropertyFeature, ServicePropertyFeature>();

            // Add this if not already there
            builder.Services.AddScoped<IServiceCity, ServiceCity>();
            builder.Services.AddScoped<IServicePropertyStatus, ServicePropertyStatus>();
            builder.Services.AddScoped<AccountService>();
            //builder.Services.AddScoped<IEmailSender,SmtpEmailSender>();
            //builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

            // Add session support for admin authentication
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            //// SEED USERS
            //// ⭐ RUN SEEDING HERE
            //using (var scope = app.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //    await DefaultUsersSeeder.SeedAsync(userManager);
            //}

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/App/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();
            
            // Enable session middleware
            app.UseSession();
            
            app.UseAuthentication(); // Add this line to enable authentication
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                //pattern: "{controller=Home}/{action=Index}/{id?}")
                pattern: "{controller=App}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.Run();
        }
    }
}
