using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luna2000.Models;

public class PhotoEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid FileId { get; set; }

    public string FileName { get; set; }

    public string FileExtension { get; set; }

    public Guid? DriverId { get; set; }

    [ForeignKey(nameof(DriverId))]
    public virtual DriverEntity? Driver { get; set; }

    public Guid? CarId { get; set; }

    [ForeignKey(nameof(CarId))]
    public virtual CarEntity? Car { get; set; }
}