using Microsoft.AspNetCore.Mvc;

namespace luna2000.Dto;

public class AddCarRequest
{
    public string Id { get; set; } = default!;

    public string BrandModel { get; set; } = default!;

    public string Pts { get; set; } = default!;

    public string Sts { get; set; } = default!;

    public string Vin { get; set; } = default!;

    public int Year { get; set; } = default!;

    public DateOnly RegistrationDate { get; set; } = default!;

    public string PlateNumber { get; set; } = default!;

    public string Osago { get; set; } = default!;

    public string Kasko { get; set; } = default!;

    public DateOnly TechInspection { get; set; } = default!;

    public string TaxiLicense { get; set; } = default!;

    public bool PurchaseOrRent { get; set; }

    public bool Leasing { get; set; }
}
