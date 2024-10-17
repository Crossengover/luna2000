namespace luna2000.Models;

public class UpdateRentRequest
{
    public Guid EditRentId { get; set; }

    public decimal Rent { get; set; }
}