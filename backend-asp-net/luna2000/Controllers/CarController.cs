using AutoMapper;
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
    private readonly IMapper _mapper;

    public CarController(LunaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var cars = _dbContext.Set<CarEntity>()
            .Include(entity => entity.Photos)
            .AsNoTracking()
            .ToArray();
        return View(cars);
    }

    [HttpGet]
    [Route("add")]
    public IActionResult AddCar()
    {
        return View("Add");
    }

    [Route("/save-car")]
    [HttpPost]
    public async Task<IActionResult> SaveCar(AddCarRequest request)
    {
        var car = _mapper.Map<CarEntity>(request);

        _dbContext.Set<CarEntity>().Add(car);
        await _dbContext.SaveChangesAsync();

        return Json(request);
    }
}