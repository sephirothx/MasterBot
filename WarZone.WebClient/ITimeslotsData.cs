using System.Collections.Generic;

namespace WarZone.WebClient
{
    public interface ITimeslotsData
    {
        List<string> GetTimeslotTemplates(int id);
    }
}
