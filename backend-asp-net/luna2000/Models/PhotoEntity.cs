using System.ComponentModel.DataAnnotations;

namespace luna2000.Models;

public class PhotoEntity
{
    [Key]
    public int Id { get; set; }

    public string Path { get; set; }

    public int DriverId { get; set; }

    public virtual DriverEntity Driver { get; set; }
}