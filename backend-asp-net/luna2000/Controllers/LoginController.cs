using System.Security.Claims;
using System.Text.Json;
using luna2000.Data;
using luna2000.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace luna2000.Controllers;

[Route("/login")]
public class LoginController : Controller
{
    private readonly LunaDbContext _dbContext;

    public LoginController(LunaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _dbContext.Set<UserEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity =>
                entity.Login == loginRequest.Username && entity.Password == loginRequest.Password);

        if (user == null)
        {
            return NotFound(JsonSerializer.Serialize("incorrect login/password"));
        }

        var claim = new Claim(ClaimTypes.Name, user.Login);
        var ci = new ClaimsIdentity(new[] { claim }, CookieAuthenticationDefaults.AuthenticationScheme);
        ci.AddClaim(claim);
        var cp = new ClaimsPrincipal(ci);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp);

        return Ok(JsonSerializer.Serialize(new { success = true }));
    }
}