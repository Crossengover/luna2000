using luna2000.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace luna2000.Logs.Impl;

public class DriverEntityLogMessageGenerator : ILogMessageGenerator
{
    public Type GetTypeOfEntity()
    {
        return typeof(DriverEntity);
    }

    public string GenerateMessage(EntityEntry entry, EventType eventType)
    {
        if (entry.Entity is DriverEntity driver)
        {
            return eventType switch
            {
                EventType.Add => $"Добавлен водитель {driver.Fio}",
                EventType.Delete => $"Удален водитель {driver.Fio}",
                EventType.Edit => $"Изменен водитель {driver.Fio}",
                _ => $"Водитель {driver.Fio}"
            };
        }

        throw new ArgumentException(entry.Entity.GetType().FullName);
    }
}