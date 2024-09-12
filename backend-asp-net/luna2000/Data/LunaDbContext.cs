using luna2000.Models;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Data;

public class LunaDbContext : DbContext
{
    public DbSet<DriverEntity> Drivers { get; set; }

    public DbSet<PhotoEntity> Photos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DB\\\\data.db");
        base.OnConfiguring(optionsBuilder);
    }
}
