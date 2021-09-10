using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Chrome;

namespace WarZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeslotsController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public ActionResult<List<string>> GetTimeslotTemplates(int id)
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
