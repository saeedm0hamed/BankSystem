using Bank.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bank.Web.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Bank.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/identity/account/login";
                options.LogoutPath = $"/identity/account/logout";
                options.AccessDeniedPath = $"/identity/account/accessdenied";
            });
            builder.Services.AddRazorPages();
            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Seed database and auto-apply migrations on run
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                db.Database.Migrate();

                var services = scope.ServiceProvider;
                try
                {
                    await DatabaseSeeder.InitializeAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // HTTP request pipeline configuration
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

/* 
    User Authentication & Authorization (Session/JWT-based) < Done
    ViewModel
    async task
    Payment Integration (Stripe)
    Repository Pattern and Unit Of Work
    API
    CSS
    n8n

 */