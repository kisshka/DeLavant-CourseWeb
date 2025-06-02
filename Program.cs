using MongoDB.Driver;
namespace DeLavant_CourseWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase("CourseDb");

            builder.Services.AddSingleton(mongoClient);
            builder.Services.AddSingleton(database);

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Course}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
