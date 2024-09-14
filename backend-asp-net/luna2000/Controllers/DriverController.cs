using AutoMapper;
using luna2000.Data;
using luna2000.Dto;
using luna2000.Models;
using luna2000.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Controllers;

public class DriverController : Controller
{
    private readonly LunaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;

    public DriverController(LunaDbContext dbContext, IMapper mapper, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    public IActionResult Index()
    {
        var drivers = _dbContext.Set<DriverEntity>()
            .Include(d => d.Photos)
            .AsNoTracking()
            .ToArray();

        return View(drivers.ToArray());
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] AddDriverRequest request)
    {
        var driver = _mapper.Map<DriverEntity>(request);
        driver.Photos = new List<PhotoEntity>();

        if (request.Photos != null)
        {
            foreach (var photo in request.Photos)
            {
                await using var memStream = new MemoryStream();
                await photo.CopyToAsync(memStream);
                var driverPhoto = _mapper.Map<IFormFile, PhotoEntity>(photo);

                var fileId = await _fileStorage.SaveFileAsync(memStream.ToArray(), driverPhoto.FileExtension);

                driverPhoto.FileId = fileId;

                driver.Photos.Add(driverPhoto);
            }
        }

        await _dbContext.AddAsync(driver);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}