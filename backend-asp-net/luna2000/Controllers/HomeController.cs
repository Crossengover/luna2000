using System.Data.Entity;
using luna2000.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using luna2000.Data;
using luna2000.Dto;
using Microsoft.AspNetCore.Authorization;

namespace luna2000.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly LunaDbContext _dbContext;

    public HomeController(LunaDbContext dbContext)
    {
        _dbContext = dbContext;
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
                .AsNoTracking()
                .ToArray()
        };

        return View(mainDto);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}