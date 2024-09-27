using AutoMapper;
using luna2000.Data;
using luna2000.Dto;
using luna2000.Models;
using luna2000.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Controllers;

[Authorize]
[Route("/car")]
public class CarController : Controller
{
    private readonly LunaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;

    public CarController(LunaDbContext dbContext, IMapper mapper, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _fileStorage = fileStorage;
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
        car.Photos = new List<PhotoEntity>();

        if (request.Photos != null)
        {
            foreach (var photo in request.Photos)
            {
                await using var memStream = new MemoryStream();
                await photo.CopyToAsync(memStream);
                var carPhoto = _mapper.Map<IFormFile, PhotoEntity>(photo);

                var fileId = await _fileStorage.SaveFileAsync(memStream.ToArray(), carPhoto.FileExtension);

                carPhoto.FileId = fileId;

                car.Photos.Add(carPhoto);
            }
        }

        _dbContext.Set<CarEntity>().Add(car);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}