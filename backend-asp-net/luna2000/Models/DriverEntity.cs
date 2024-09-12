using System.ComponentModel.DataAnnotations;

namespace luna2000.Models;

public class DriverEntity
{
    [Key]
    public string Id { get; set; }

    public string Address { get; set; }

    public string Contacts { get; set; }

    public string DriverLicense { get; set; }

    public string Fio { get; set; }

    public string Note { get; set; }

    public string ParkingAddress { get; set; }

    public string Passport { get; set; }

    public string Registration { get; set; }

    public virtual ICollection<PhotoEntity>? Photos { get; set; }
}
