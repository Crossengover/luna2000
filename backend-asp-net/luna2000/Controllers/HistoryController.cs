using AutoMapper;
using luna2000.Data;
using luna2000.Dto;
using luna2000.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Controllers;

[Authorize]
public class HistoryController : Controller
{
    private readonly LunaDbContext _dbContext;
    private readonly IMapper _mapper;

    public HistoryController(LunaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var logs = _dbContext
            .Set<BaseLog>()
            .AsNoTracking()
            .ToArray();

        return View(_mapper.Map<IEnumerable<HistoryDto>>(logs).GroupBy(dto => dto.ChangeId));
    }
}