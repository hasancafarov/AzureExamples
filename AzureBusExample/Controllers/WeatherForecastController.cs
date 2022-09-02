using AzureBusExample.Models;
using AzureBusExample.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBusExample.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class WeatherForecastController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAzureBusService _busService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAzureBusService busService)
        {
            _logger = logger;
            _busService = busService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Person person)
        {
            await _busService.SendMessageAsync(person, "mynewqueue");
            return Ok("Privacy");
        }

    }
}