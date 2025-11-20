namespace Estately.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddControllersWithViews();
            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //Add Context with Connection 
            builder.Services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IServiceUser, ServiceUser>();
            builder.Services.AddScoped<IServiceProperty, ServiceProperty>();
            builder.Services.AddScoped<IServiceZone, ServiceZone>();
            builder.Services.AddScoped<IServiceUserType, ServiceUserType>();
            builder.Services.AddScoped<IServiceDepartment, ServiceDepartment>();
            builder.Services.AddScoped<IServiceBranch, ServiceBranch>();
            builder.Services.AddScoped<IServiceEmployee, ServiceEmployee>();
            builder.Services.AddScoped<IServicePropertyType, ServicePropertyType>();

            // Add this if not already there
            builder.Services.AddScoped<IServiceCity, ServiceCity>();



            //builder.Services.AddScoped<IServiceProperty, ServiceProperty>();
            //builder.Services.AddScoped<IServiceZone, ServiceZone>();
            //// Add this line to register your service
            //builder.Services.AddScoped<IServiceUserType, ServiceUserType>();
            builder.Services.AddScoped<IServicePropertyStatus, ServicePropertyStatus>();

            // Add session support for admin authentication
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
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
