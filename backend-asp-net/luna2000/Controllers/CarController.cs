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
    public IActionResult Index()
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

        return Ok(new { success = true });
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public IActionResult Delete(Guid id)
    {
        var car = _dbContext.Set<CarEntity>()
            .FirstOrDefault(entity => entity.Id == id);

        if (car == null)
        {
            return NotFound();
        }

        _dbContext.Remove(car);
        _dbContext.SaveChanges();

        return Ok(new { success = true });
    }

    [HttpGet]
    [Route("edit")]
    public IActionResult Edit([FromQuery] Guid id)
    {
        var car = _dbContext.Set<CarEntity>()
            .Include(entity => entity.Photos)
            .FirstOrDefault(entity => entity.Id == id);

        if (car == null)
        {
            return Redirect("/");
        }

        return View(car);
    }

    [HttpPost]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(AddCarRequest request)
    {
        var car = await _dbContext.Set<CarEntity>()
            .Include(entity => entity.Photos)
            .FirstOrDefaultAsync(entity => entity.Id == request.Id);

        if (car == null)
        {
            return NotFound();
        }

        _mapper.Map(request, car);

        if (request.Photos != null && request.Photos.Count != 0)
        {
            DeleteCarPhotos(car.Photos);
            car.Photos = new List<PhotoEntity>();

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

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    private void DeleteCarPhotos(ICollection<PhotoEntity> photos)
    {
        foreach (var photo in photos)
        {
            _fileStorage.DeletePhoto(photo.FileId);
        }
    }
}