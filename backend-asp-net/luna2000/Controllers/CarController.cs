using luna2000.Dto;
using Microsoft.AspNetCore.Mvc;

namespace luna2000.Controllers;

[Route("/car")]
public class CarController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("/save-car")]
    [HttpPost]
    public IActionResult SaveCar(SaveCarRequest request)
    {
        return Json(request);
    }
}