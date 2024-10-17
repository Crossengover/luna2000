using luna2000.Data;
using luna2000.Models;
using luna2000.Service;
using Microsoft.AspNetCore.Mvc;

namespace luna2000.Controllers;

[Route("api")]
public class ApiController : ControllerBase
{
    private readonly IDeductRentService _deductRentService;
    private readonly LunaDbContext _dbContext;

    public ApiController(IDeductRentService deductRentService, LunaDbContext dbContext)
    {
        _deductRentService = deductRentService;
        _dbContext = dbContext;
    }

    [HttpPost("deduct-rent")]
    public IActionResult ProcessDbTask()
    {
        _deductRentService.DeductRent();

        _dbContext.Set<BaseLog>().Add(new BaseLog()
        {
            ChangeId = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            EventType = EventType.Edit,
            Note = "Автоматический вызов списания аренды"
        });

        _dbContext.SaveChanges();

        return Ok();
    }
}