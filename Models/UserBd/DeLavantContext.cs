using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeLavant_CourseWeb.Models.UserBd;

public class DeLavantContext : IdentityDbContext<User>
{

    public DbSet<Post> Posts { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DeLavantContext(DbContextOptions<DeLavantContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasMany(p => p.Posts)
            .WithMany(u => u.Users)
            .UsingEntity(b => b.ToTable("UserPosts"));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Users.db");
    }
}
