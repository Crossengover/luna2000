using luna2000.Data;
using luna2000.Dto;
using luna2000.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace luna2000.Controllers;

[Authorize]
public class DocEditorController : Controller
{
    private readonly LunaDbContext _dbContext;

    public DocEditorController(LunaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var model = new EditorViewDto
        {
            Drivers = _dbContext.Set<DriverEntity>()
                .AsNoTracking()
                .ToArray()
                .Select(entity => (entity.Id, entity.Fio))
                .ToArray(),
            Cars = _dbContext.Set<CarEntity>()
                .AsNoTracking()
                .ToArray()
                .Select(entity => (entity.Id, $"{entity.BrandModel} ({entity.PlateNumber})"))
                .ToArray(),
            FileNames = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Doc"))
                .Select(Path.GetFileName).ToArray()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult EditFile(string fileName, Guid driverId, Guid carId)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, "Doc", fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var driver = _dbContext.Set<DriverEntity>()
            .AsNoTracking()
            .FirstOrDefault(entity => entity.Id == driverId);

        var car = _dbContext.Set<CarEntity>()
            .AsNoTracking()
            .FirstOrDefault(entity => entity.Id == carId);

        if (car == null || driver == null)
        {
            return NotFound();
        }

        using var fileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read);
        using var document = DocX.Load(fileStream);
        var memoryStream = EditDocument(document, driver, car);

        return File(memoryStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            $"Договор для {driver.Fio}.docx");
    }

    private MemoryStream EditDocument(DocX document, DriverEntity driver, CarEntity car)
    {
        var replaceDictionary = new Dictionary<string, string>
        {
            {"{fio}", driver.Fio},
            {"{passport}", driver.Passport},
            {"{registration}", driver.Registration},
            {"{contacts}", driver.Contacts},
            {"{brandModel}", car.BrandModel},
            {"{vin}", car.Vin},
            {"{year}", car.Year},
            {"{pts}", car.Pts},
            {"{sts}", car.Sts},
            {"{plateNumber}", car.PlateNumber},
            {"{osago}", car.Osago},
        };

        foreach (var oldText in replaceDictionary.Keys)
        {
            document.ReplaceText(new StringReplaceTextOptions()
            {
                SearchValue = oldText,
                NewValue = replaceDictionary[oldText]
            });
        }

        var memoryStream = new MemoryStream();
        document.SaveAs(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
}