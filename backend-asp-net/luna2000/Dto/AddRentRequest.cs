namespace luna2000.Dto;

public class AddRentRequest
{
    public Guid CarId { get; set; }

    public Guid DriverId { get; set; }

    public decimal Rent { get; set; }
}