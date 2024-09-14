using System.ComponentModel.DataAnnotations;

namespace luna2000.Models;

public class CarEntity
{
    [Key]
    public Guid Id { get; set; }

    public string BrandModel { get; set; }

    public string Pts { get; set; }

    public string Sts { get; set; }

    public string Vin { get; set; }

    public string Year { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public string PlateNumber { get; set; }

    public string Osago { get; set; }

    public string Kasko { get; set; }

    public DateOnly TechInspection { get; set; }

    public bool TaxiLicense { get; set; }

    public string PurchaseOrRent { get; set; }

    public bool Leasing { get; set; }

    public virtual ICollection<PhotoEntity>? Photos { get; set; }
}