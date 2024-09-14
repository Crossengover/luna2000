using luna2000.Models;

namespace luna2000.Dto;

public class MainViewDto
{
    public DriverEntity[] Drivers { get; set; }

    public CarEntity[] Cars { get; set; }

    public CarRentalEntity[] Rentals { get; set; }
}