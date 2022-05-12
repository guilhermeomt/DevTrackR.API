using Microsoft.EntityFrameworkCore;
using DevTrackR.API.Entities;

namespace DevTrackR.API.Persistance
{
  public class DevTrackRContext : DbContext
  {
    public DevTrackRContext(DbContextOptions<DevTrackRContext> options)
     : base(options)
    {

    }

    public DbSet<Package> Packages { get; set; }
    public DbSet<PackageUpdate> PackageUpdates { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Package>(e =>
      {
        e.ToTable("packages");
        e.HasKey(p => p.Id);

        e.HasMany(p => p.Updates)
         .WithOne()
         .HasForeignKey(pu => pu.PackageId)
         .OnDelete(DeleteBehavior.Restrict);
      });

      builder.Entity<PackageUpdate>(e =>
      {
        e.ToTable("package_updates");
        e.HasKey(p => p.Id);
      });


    }
  }
}