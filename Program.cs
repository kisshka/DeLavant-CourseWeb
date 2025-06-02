using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DeLavant_CourseWeb.Models.UserBd;
namespace DeLavant_CourseWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DeLavantContextConnection") ?? throw new InvalidOperationException("Connection string 'DeLavantContextConnection' not found.");

            builder.Services.AddDbContext<DeLavantContext>(options => options.UseSqlite(connectionString));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DeLavantContext>()
            .AddDefaultTokenProviders();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
