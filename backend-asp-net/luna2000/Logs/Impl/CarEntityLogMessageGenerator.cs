using luna2000.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace luna2000.Logs.Impl;

public class CarEntityLogMessageGenerator : ILogMessageGenerator
{
    public Type GetTypeOfEntity()
    {
        return typeof(CarEntity);
    }

    public string GenerateMessage(EntityEntry entry, EventType eventType)
    {
        if (entry.Entity is CarEntity car)
        {
            return eventType switch
            {
                EventType.Add => $"Добавлен автомобиль {car.BrandModel} ({car.PlateNumber})",
                EventType.Delete => $"Удален автомобиль {car.BrandModel} ({car.PlateNumber})",
                EventType.Edit => $"Изменен автомобиль {car.BrandModel} ({car.PlateNumber})",
                _ => $"Автомобиль {car.BrandModel}  ( {car.PlateNumber} )"
            };
        }

        throw new ArgumentException(entry.Entity.GetType().FullName);
    }
}