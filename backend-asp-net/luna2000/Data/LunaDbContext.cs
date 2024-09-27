using luna2000.Models;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Data;

public class LunaDbContext : DbContext
{
    public DbSet<DriverEntity> Drivers { get; set; }

    public DbSet<PhotoEntity> Photos { get; set; }

    public DbSet<CarEntity> Cars { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<CarRentalEntity> CarRentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DB\\\\data.db");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DriverEntity>()
            .HasMany(d => d.Photos)
            .WithOne(p => p.Driver)
            .IsRequired(false);

        modelBuilder.Entity<CarEntity>()
            .HasMany(d => d.Photos)
            .WithOne(p => p.Car)
            .IsRequired(false);

        modelBuilder.Entity<PhotoEntity>()
            .HasOne(p => p.Driver)
            .WithMany(d => d.Photos)
            .HasForeignKey(p => p.DriverId)
            .IsRequired(false);

        modelBuilder.Entity<PhotoEntity>()
            .HasOne(p => p.Car)
            .WithMany(d => d.Photos)
            .HasForeignKey(p => p.CarId)
            .IsRequired(false);
    }

    public void ExecuteMigrationScript()
    {
        var migrationScriptPath = Path.Combine(Environment.CurrentDirectory, "Other", "MigrationScript.sql");

        if (!File.Exists(migrationScriptPath))
        {
            return;
        }

        var sqlScript = File.ReadAllText(migrationScriptPath);

        if (string.IsNullOrEmpty(sqlScript))
        {
            return;
        }

        Database.ExecuteSqlRaw(sqlScript);
    }
}
