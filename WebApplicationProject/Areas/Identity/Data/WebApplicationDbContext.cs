using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApplicationProject.Areas.Identity.Data;
using WebApplicationProject.Models;

namespace WebApplicationProject.Data;

public class WebApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    public DbSet<UserEvent> UserEvents { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        var modelBuilder = new ModelBuilder(builder.Model);
        modelBuilder.Entity<UserEvent>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.UserEvents)
            .HasForeignKey(ue => ue.UserID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEvent>()
            .HasOne(ue => ue.Event)
           .WithMany(e => e.UserEvents)
            .HasForeignKey(ue => ue.EventID)
            .OnDelete(DeleteBehavior.Restrict);




    }
}
