using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WarZone.WebClient
{
    public class TimeslotsData : ITimeslotsData
    {
        public List<string> GetTimeslotTemplates(int id)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");

            using var driver = new ChromeDriver(chromeOptions);
            driver.Url = $"https://www.warzone.com/Clans/War?ID=6&Timeslot={id}";
            var elements = driver.FindElements(By.CssSelector("[id^=ujs_TemplateNameLabel][id$=tmp]"));
            
            return !elements.Any()
                       ? new List<string>()
                       : elements.Select(e => e.Text).ToList();
        }
    }
}
