using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DeLavant_CourseWeb.Models.UserBd;
using MongoDB.Driver;
namespace DeLavant_CourseWeb
{
    public class Program
    {
        public static void Main(string[] args)
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

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();
            app.MapRazorPages();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Accounts}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
