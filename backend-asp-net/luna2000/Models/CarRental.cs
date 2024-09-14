using System.ComponentModel.DataAnnotations;

namespace luna2000.Models;

public class CarRentalEntity
{
    [Key]
    public long Id { get; set; }

    public string DriverId { get; set; }

    public string CarId { get; set; }

    public string DriverName { get; set; }

    public string CarName { get; set; }

    public decimal Balance { get; set; }

    public decimal Rent { get; set; }

    public virtual DriverEntity Driver { get; set; }

    public virtual CarEntity Car { get; set; }
}