using luna2000.Data;
using luna2000.Models;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Service;

public class DeductRentService : IDeductRentService
{
    private readonly LunaDbContext _dbContext;

    public DeductRentService(LunaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void DeductRent()
    {
        var carRentals = _dbContext.Set<CarRentalEntity>()
            .Include(entity => entity.Driver)
            .ToArray();

        foreach (var carRental in carRentals)
        {
            carRental.Driver!.Balance -= carRental.Rent;
        }

        _dbContext.SaveChanges();
    }
}