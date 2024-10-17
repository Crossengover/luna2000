using luna2000.Data;
using luna2000.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace luna2000.Logs.Impl;

public class CarRentalEntityLogMessageGenerator : ILogMessageGenerator
{
    private readonly IDbContextFactory<LunaDbContext> _dbContextFactory;

    public CarRentalEntityLogMessageGenerator(IDbContextFactory<LunaDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Type GetTypeOfEntity()
    {
        return typeof(CarRentalEntity);
    }

    public string GenerateMessage(EntityEntry entry, EventType eventType)
    {
        if (entry.Entity is not CarRentalEntity carRent)
        {
            throw new ArgumentException(entry.Entity.GetType().FullName);
        }

        using var dbContext = _dbContextFactory.CreateDbContext();

        var driver = dbContext.Set<DriverEntity>()
            .AsNoTracking()
            .FirstOrDefault(entity => entity.Id == carRent.DriverId);

        var car = dbContext.Set<CarEntity>()
            .AsNoTracking()
            .FirstOrDefault(entity => entity.Id == carRent.CarId);

        return eventType switch
        {
            EventType.Add => $"Добавлена запись аренды {car?.BrandModel} ({car?.PlateNumber}) + {driver?.Fio}",
            EventType.Delete => $"Удалена запись аренды {car?.BrandModel} ({car?.PlateNumber}) + {driver?.Fio}",
            EventType.Edit => $"Изменена запись аренды {car?.BrandModel} ({car?.PlateNumber}) + {driver?.Fio}",
            _ => $"Запись аренды {car?.BrandModel} ({car?.PlateNumber}) + {driver?.Fio}"
        };
    }
}