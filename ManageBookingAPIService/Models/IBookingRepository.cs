using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageBookingAPIService.Models
{
    public interface IBookingRepository
    {
        bool BookTicket(Booking ticketDetails);

        IEnumerable<Booking> GetBookedTicket(int pnrNumber);

        IEnumerable<Booking> GetBookingHistory(string emailId);

        bool CancelBookedTicket(int bookingRecId);

    }
}
