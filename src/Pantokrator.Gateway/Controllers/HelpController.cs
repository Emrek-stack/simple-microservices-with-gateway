using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Pantokrator.Gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/healthcheck")]

    public class HelpController : Controller
    {
        private readonly ILogger<HelpController> _logger;

        public HelpController(ILogger<HelpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}