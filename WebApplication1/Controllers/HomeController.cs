using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    private static int _count = 0;

    [HttpGet("count/{multiplier:int}")]
    public IActionResult Count([FromRoute] int multiplier)
    {
        _logger.LogInformation("Count called with multiplier {multiplier}", multiplier);
        return Json(new
        {
            count = _count++ * multiplier
        });
    }
}