using luna2000.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace luna2000.Logs.Impl;

public class PhotoEntityLogMessageGenerator : ILogMessageGenerator
{
    public Type GetTypeOfEntity()
    {
        return typeof(PhotoEntity);
    }

    public string GenerateMessage(EntityEntry entry, EventType eventType)
    {
        if (entry.Entity is PhotoEntity photo)
        {
            return eventType switch
            {
                EventType.Add => $"Добавлено фото {photo.FileName}",
                EventType.Delete => $"Удалено фото {photo.FileName})",
                EventType.Edit => $"Изменено фото {photo.FileName}",
                _ => $"Фото {photo.FileName}"
            };
        }

        throw new ArgumentException(entry.Entity.GetType().FullName);
    }
}