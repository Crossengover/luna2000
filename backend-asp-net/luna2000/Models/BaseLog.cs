using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace luna2000.Models;

public class BaseLog
{
    [Key]
    public Guid Id { get; set; }

    public string? PropertyName { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    /// <summary>
    /// Id обновления, по которому можно объединять группу разных обновлений объекта
    /// </summary>
    public Guid ChangeId { get; set; }

    public DateTime Created { get; set; }

    public EventType EventType { get; set; }

    public Guid? EntryId { get; set; }

    public string? ObjectName { get; set; }

    public string? Note { get; set; }
}

public enum EventType
{
    [Description("Создание")]
    Add,
    [Description("Удаление")]
    Delete,
    [Description("Редактирование")]
    Edit,
    [Description("Событие")]
    Event
}
