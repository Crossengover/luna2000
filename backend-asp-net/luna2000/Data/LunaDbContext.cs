using luna2000.Logs;
using luna2000.Models;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Data;

public class LunaDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;

    public DbSet<DriverEntity> Drivers { get; set; }

    public DbSet<PhotoEntity> Photos { get; set; }

    public DbSet<CarEntity> Cars { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<CarRentalEntity> CarRentals { get; set; }

    public DbSet<BaseLog> BaseLogs { get; set; }

    public LunaDbContext(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={Path.Combine("DB", "data.db")}");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DriverEntity>()
            .HasMany(d => d.Photos)
            .WithOne(p => p.Driver)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        modelBuilder.Entity<CarEntity>()
            .HasMany(d => d.Photos)
            .WithOne(p => p.Car)
            .OnDelete(DeleteBehavior.Cascade)
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

    public override int SaveChanges()
    {
        TrackChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        TrackChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void TrackChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added or EntityState.Deleted);

        var logs = new List<BaseLog>();

        foreach (var entry in entries)
        {
            if (entry.Metadata.ClrType == typeof(BaseLog))
            {
                return;
            }

            var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue;

            var changeId = Guid.NewGuid();
            var logFactory = _serviceProvider.GetService<ILogMessageGeneratorFactory>();

            if (logFactory == null)
            {
                throw new InvalidOperationException();
            }

            switch (entry.State)
            {
                case EntityState.Deleted:
                    {
                        logs.Add(new BaseLog
                        {
                            EntryId = entityId is Guid id ? id : null,
                            ChangeId = changeId,
                            Created = DateTime.UtcNow,
                            EventType = EventType.Delete,
                            Note = logFactory.GetGenerator(entry.Metadata.ClrType)?
                                .GenerateMessage(entry, EventType.Delete)
                        });
                        break;
                    }
                case EntityState.Modified:
                    {
                        foreach (var property in entry.Properties)
                        {
                            if (!property.IsModified)
                            {
                                continue;
                            }

                            logs.Add(new BaseLog
                            {
                                EntryId = entityId is Guid id ? id : null,
                                PropertyName = property.Metadata.Name,
                                OldValue = property.OriginalValue?.ToString(),
                                NewValue = property.CurrentValue?.ToString(),
                                ChangeId = changeId,
                                EventType = EventType.Edit,
                                Created = DateTime.UtcNow,
                                ObjectName = property.Metadata.Name,
                                Note = logFactory.GetGenerator(entry.Metadata.ClrType)?
                                     .GenerateMessage(entry, EventType.Edit)
                            });
                        }
                        break;
                    }
                case EntityState.Added:
                    {
                        logs.Add(new BaseLog
                        {
                            EntryId = entityId is Guid id ? id : null,
                            ChangeId = changeId,
                            Created = DateTime.UtcNow,
                            EventType = EventType.Add,
                            Note = logFactory.GetGenerator(entry.Metadata.ClrType)?
                                .GenerateMessage(entry, EventType.Add)
                        });
                        break;
                    }
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    throw new ArgumentOutOfRangeException(entry.State.ToString());
            }
        }

        if (logs.Count != 0)
        {
            BaseLogs.AddRange(logs);
        }
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
