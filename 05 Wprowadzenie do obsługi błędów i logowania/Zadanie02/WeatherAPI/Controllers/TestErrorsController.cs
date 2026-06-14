using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestErrorsController : ControllerBase
{
    private readonly ILogger<TestErrorsController> _logger;

    public TestErrorsController(ILogger<TestErrorsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Zgłoszono testowy wyjątek");
        throw new Exception("Testowy wyjątek");
    }
}
