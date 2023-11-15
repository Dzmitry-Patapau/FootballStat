using FootballStats.Data;
using FootballStats.Data.Identity;
using FootballStats.Services;
using FootballStats.Services.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace FootballStats
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IImageService,ImageService>();
            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddSession();
            builder.Services.AddRazorPages();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.MapBlazorHub();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "api",
                pattern: "api/{controller}/{action}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var _usermanager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roles = new string[] { "Administrator", "Moderator", "User" };
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                var admin = await _usermanager.FindByNameAsync("Admin@bgut.by");
                if (admin == null)
                {
                    var adminModel = new ApplicationUser
                    {
                        UserName = "Admin@test.by",
                        Email = "Admin@test.by",
                        NickName = "Adminus",
                    };
                    await _usermanager.CreateAsync(adminModel, "Test1234,");
                    await _usermanager.AddToRoleAsync(adminModel, "Administrator");
                }

            }

            app.Run();
        }
    }
}