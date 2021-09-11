using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace WarZone.Web
{
    public interface ITimeslotsData
    {
        [Get("/Timeslots/{id}")]
        Task<List<string>> GetTimeslotTemplates(int id);
    }
}
