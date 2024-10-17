using luna2000.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using luna2000.Data;
using luna2000.Dto;
using luna2000.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly LunaDbContext _dbContext;
    private readonly IDeductRentService _deductRentService;

    public HomeController(LunaDbContext dbContext, IDeductRentService deductRentService)
    {
        _dbContext = dbContext;
        _deductRentService = deductRentService;
    }

    public IActionResult Index()
    {
        var mainDto = new MainViewDto
        {
            Cars = _dbContext.Set<CarEntity>()
                .AsNoTracking()
                .ToArray(),
            Drivers = _dbContext.Set<DriverEntity>()
                .AsNoTracking()
                .ToArray(),
            Rentals = _dbContext.Set<CarRentalEntity>()
                .Include(entity => entity.Car)
                .Include(entity => entity.Driver)
                .AsNoTracking()
                .ToArray()
        };

        return View(mainDto);
    }

    [HttpPost]
    public IActionResult DeletePair([FromBody] Guid pairId)
    {
        var carRental = _dbContext.Set<CarRentalEntity>()
            .FirstOrDefault(entity => entity.Id == pairId);

        if (carRental == null)
        {
            return NotFound();
        }

        _dbContext.Set<CarRentalEntity>()
            .Remove(carRental);
        _dbContext.SaveChanges();

        return Ok();
    }

    [HttpPost]
    public IActionResult AddRent([FromBody] AddRentRequest addRentRequest)
    {
        _dbContext.Set<CarRentalEntity>()
            .Add(new CarRentalEntity()
            {
                DriverId = addRentRequest.DriverId,
                CarId = addRentRequest.CarId,
                Rent = addRentRequest.Rent
            });

        _dbContext.SaveChanges();

        return Ok();
    }

    [HttpPost]
    public IActionResult UpdateRent([FromBody] UpdateRentRequest request)
    {
        var carRental = _dbContext.Set<CarRentalEntity>()
            .FirstOrDefault(entity => entity.Id == request.EditRentId);

        if (carRental == null)
        {
            return NotFound();
        }

        carRental.Rent = request.Rent;
        _dbContext.SaveChanges();

        return Ok();
    }

    public IActionResult DeductRent()
    {
        _deductRentService.DeductRent();

        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}