using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DeLavant_CourseWeb.Models.UserBd;
using MongoDB.Driver;

namespace DeLavant_CourseWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DeLavantContextConnection") ?? throw new InvalidOperationException("Connection string 'DeLavantContextConnection' not found.");

            builder.Services.AddDbContext<DeLavantContext>(options => options.UseSqlite(connectionString));

            var connectionStringMongo = builder.Configuration.GetConnectionString("DefaultConnection");
            
            var mongoClient = new MongoClient(connectionStringMongo);
            var database = mongoClient.GetDatabase("CourseDb");

            builder.Services.AddSingleton(mongoClient);
            builder.Services.AddSingleton(database);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DeLavantContext>()
            .AddDefaultTokenProviders();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

/*            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roles = new[] { "Admin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole  (role));
                    }
                }
                var userName = "IamAdmin";
                var email = "admin@mail.com";
                var password = "Admin123!";
                var surname = "Лучшая";
                var name = "Ольга";
                var fatherName = "Александровна";
                var user = new User { UserName = userName, Email = email, UserSurName = surname, Name = name, UserFatherName = fatherName };
                var createResult = await userManager.CreateAsync(user, password);
                var addRoleResult = await userManager.AddToRoleAsync(user, "Admin");
            }*/


            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();
            app.MapRazorPages();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Division}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
