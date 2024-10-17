using luna2000.Models;

namespace luna2000.Dto;

public class HistoryDto
{
    public string? PropertyName { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public Guid ChangeId { get; set; }

    public DateTime Created { get; set; }

    public EventType EventType { get; set; }

    public Guid? EntryId { get; set; }

    public string? ObjectName { get; set; }

    public string? Note { get; set; }
}