using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;

namespace WarZone.Api.v1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TimeslotsController : ControllerBase
    {
        [HttpGet("{id:int}"), MapToApiVersion("1.0")]
        public ActionResult<List<string>> Get(int id)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");

            using var driver = new ChromeDriver(chromeOptions);
            driver.Url = $"https://www.warzone.com/Clans/War?ID=5&Timeslot={id}";
            var elements = driver.FindElementsByCssSelector("[id^=ujs_TemplateNameLabel][id$=tmp]");
            if (!elements.Any()) return NotFound();

            return elements.Select(e => e.Text).ToList();
        }
    }
}
