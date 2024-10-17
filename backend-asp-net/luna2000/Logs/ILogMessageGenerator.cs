using luna2000.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace luna2000.Logs;

public interface ILogMessageGenerator
{
    Type GetTypeOfEntity();

    string GenerateMessage(EntityEntry entry, EventType eventType);
}