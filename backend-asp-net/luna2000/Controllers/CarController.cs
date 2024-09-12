using luna2000.Data;
using luna2000.Dto;
using luna2000.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Controllers;

[Route("/car")]
public class CarController : Controller
{
    private readonly LunaDbContext _dbContext;

    public CarController(LunaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        var drivers = _dbContext.Set<DriverEntity>().AsNoTracking().ToArray();
        return View(drivers);
    }

    [Route("/save-car")]
    [HttpPost]
    public IActionResult SaveCar(SaveCarRequest request)
    {
        return Json(request);
    }
}