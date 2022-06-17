using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageSchedulesAPIService.Models
{
    public interface IScheduleRepository
    {
        Schedule AddSchedule(Schedule flightSchedules);

        IEnumerable<Schedule> GetSchedulesByFilter(Schedule searchFilter);

        void UpdateSeats(RabbitMqTicket ticketBookingDetails);

        IEnumerable<Schedule> GetAllSchedules();
    }
}
