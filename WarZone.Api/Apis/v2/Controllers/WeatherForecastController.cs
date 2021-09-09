using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WarZone.Api.v2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TimeslotsController : ControllerBase
    {
        private readonly ILogger<TimeslotsController> _logger;

        public TimeslotsController(ILogger<TimeslotsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public bool Get()
        {
            return false;
        }
    }
}
