using Microsoft.AspNetCore.Mvc;

namespace luna2000.Dto;

public class AddCarRequest
{
    public Guid Id { get; set; } = default!;

    public string BrandModel { get; set; } = default!;

    public string Pts { get; set; } = default!;

    public string Sts { get; set; } = default!;

    public string Vin { get; set; } = default!;

    public int Year { get; set; } = default!;

    public DateTime RegistrationDate { get; set; } = default!;

    public string PlateNumber { get; set; } = default!;

    public string Osago { get; set; } = default!;

    public string Kasko { get; set; } = default!;

    public DateTime TechInspection { get; set; } = default!;

    public string TaxiLicense { get; set; } = default!;

    public string PurchaseOrRent { get; set; }

    public bool Leasing { get; set; }

    public virtual ICollection<IFormFile>? Photos { get; set; }
}
