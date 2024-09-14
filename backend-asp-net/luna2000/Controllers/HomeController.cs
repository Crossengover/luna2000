using System.Data.Entity;
using luna2000.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using luna2000.Data;
using luna2000.Dto;

namespace luna2000.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LunaDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, LunaDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var mainDto = new MainViewDto();

            mainDto.Cars = _dbContext.Set<CarEntity>()
                .AsNoTracking()
                .ToArray();

            mainDto.Drivers = _dbContext.Set<DriverEntity>()
                .AsNoTracking()
                .ToArray();

            mainDto.Rentals = _dbContext.Set<CarRentalEntity>()
                .AsNoTracking()
                .ToArray();

            return View(mainDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}