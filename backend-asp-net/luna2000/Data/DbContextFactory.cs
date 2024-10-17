using Microsoft.EntityFrameworkCore;

namespace luna2000.Data;

public class DbContextFactory : IDbContextFactory<LunaDbContext>
{
    private readonly IServiceProvider _serviceProvider;

    public DbContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public LunaDbContext CreateDbContext()
    {
        return new LunaDbContext(_serviceProvider);
    }
}