using Estately.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            // Ensure [Authorize] redirects to the correct login endpoint
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Accounts/Login";
                // Make authentication cookie persistent
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Authentication ticket expires after 30 days
                options.SlidingExpiration = true; // Reset expiration on each request
                options.Cookie.HttpOnly = true; // Prevent JavaScript access for security
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use HTTPS in production
                options.Cookie.SameSite = SameSiteMode.Lax; // CSRF protection
                options.Cookie.IsEssential = true; // Mark as essential cookie
                
                // Use events to set cookie expiration dynamically - make all cookies persistent
                options.Events = new CookieAuthenticationEvents
                {
                    OnSigningIn = async context =>
                    {
                        // Always set cookie expiration to make it persistent (survives browser restarts)
                        context.Properties.IsPersistent = true;
                        context.CookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(30);
                        await Task.CompletedTask;
                    }
                };
            });
            //builder.Services.AddControllersWithViews();
            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

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

            builder.Services.AddScoped<IServiceCity, ServiceCity>();
            builder.Services.AddScoped<IServicePropertyStatus, ServicePropertyStatus>();
            builder.Services.AddScoped<IServiceUser, ServiceUser>();
            builder.Services.AddScoped<AccountService>();
            //builder.Services.AddScoped<IEmailSender,SmtpEmailSender>();
            //builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

            // Add session support for admin authentication
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30); // Session timeout - session expires after 30 days of inactivity
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use HTTPS in production
                options.Cookie.SameSite = SameSiteMode.Lax; // CSRF protection
            });

            var app = builder.Build();

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
                pattern: "{controller=App}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.Run();
        }
    }
}
