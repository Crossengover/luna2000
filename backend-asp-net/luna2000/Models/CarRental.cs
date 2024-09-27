using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luna2000.Models;

public class CarRentalEntity
{
    [Key]
    public Guid Id { get; set; }

    public decimal Rent { get; set; }

    public Guid DriverId { get; set; }

    public Guid CarId { get; set; }

    [ForeignKey(nameof(DriverId))]
    public virtual DriverEntity? Driver { get; set; }

    [ForeignKey(nameof(CarId))]
    public virtual CarEntity? Car { get; set; }
}