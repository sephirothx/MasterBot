using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace RefitTest
{
    public interface ITimeslots
    {
        [Get("/v1/timeslots/{id}")]
        Task<List<string>> GetTimeSlotTemplates(int id);
    }
}
