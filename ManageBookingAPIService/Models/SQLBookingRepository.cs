using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ManageBookingAPIService.Models
{
    public class SQLBookingRepository : IBookingRepository
    {
        private readonly AppDbContext _dbContext;

        private readonly IRabbitManager _rabbitManager;

        public SQLBookingRepository(AppDbContext dbContext, IRabbitManager manager)
        {
            _dbContext = dbContext;
            _rabbitManager = manager;
        }
        public bool BookTicket(Booking ticketDetails)
        {
            try
            {
                var pnrNumber = new Random().Next(10000000, 99999999);
                ticketDetails.PNRNumber = pnrNumber;
                ticketDetails.CreatedDate = DateTime.Now;
                _dbContext.Add(ticketDetails);
                _dbContext.SaveChanges();

                // Send the data to ticket queue (RabbitMQ)
                var publishData = new RabbitMqTicket()
                {
                    ScheduleRecId = ticketDetails.ScheduleRecId,
                    NoOfSeats = ticketDetails.NoOfSeats,
                    IsBooking = true,
                    IsBusinessClass = ticketDetails.IsBusinessClass,
                    IsRoundTrip = ticketDetails.IsRoundTrip,
                    ReturnScheduleRecId = ticketDetails.ReturnScheduleRecId
                };

                _rabbitManager.Publish(publishData, "Ticket");

                return true;
            }
            catch(Exception ex)
            {

                return false;
            }
        }

        public bool CancelBookedTicket(int bookingRecId)
        {
            var ticketDetails = _dbContext.Bookings.Find(bookingRecId);

            if(ticketDetails != null)
            {
                if (ticketDetails.DateOfJourney > DateTime.Now.AddHours(24))
                {
                    ticketDetails.IsCancelTicket = true;
                    _dbContext.Entry(ticketDetails).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _dbContext.SaveChanges();

                    // Send the data to ticket queue (RabbitMQ)
                    var publishData = new RabbitMqTicket
                    {
                        ScheduleRecId = ticketDetails.ScheduleRecId,
                        NoOfSeats = ticketDetails.NoOfSeats,
                        IsBooking = false,
                        IsBusinessClass = ticketDetails.IsBusinessClass,
                        IsRoundTrip = ticketDetails.IsRoundTrip,
                        ReturnScheduleRecId = ticketDetails.ReturnScheduleRecId
                    };

                    _rabbitManager.Publish(publishData, "Ticket");
                    return true;
                }

                return false;
            }

            return false;
        }

        public IEnumerable<Booking> GetBookedTicket(int pnrNumber)
        {
            var ticketDetails = (from d in _dbContext.Bookings
                                 join s in _dbContext.Schedules
                                 on d.ScheduleRecId equals s.ScheduleRecId
                                 where (d.PNRNumber == pnrNumber)
                                 select new Booking
                                 {
                                     BookingRecId = d.BookingRecId,
                                     CreatedDate = d.CreatedDate,
                                     DateOfJourney = d.DateOfJourney,
                                     EmailId = d.EmailId,
                                     IsBusinessClass = d.IsBusinessClass,
                                     IsCancelTicket = d.IsCancelTicket,
                                     IsRoundTrip = d.IsRoundTrip,
                                     Meal = d.Meal,
                                     Name = d.Name,
                                     NoOfSeats = d.NoOfSeats,
                                     Passengers = d.Passengers,
                                     PNRNumber = d.PNRNumber,
                                     ReturnDateOfJourney = d.ReturnDateOfJourney,
                                     ReturnIsBusinessClass = d.ReturnIsBusinessClass,
                                     ReturnScheduleRecId = d.ReturnScheduleRecId,
                                     ScheduleRecId = d.ScheduleRecId,
                                     Schedules = d.Schedules,
                                     Airlines = s.Airlines,
                                     TicketCost = d.TicketCost
                                 });

            return ticketDetails;
        }

        public IEnumerable<Booking> GetBookingHistory(string emailId)
        {
            var bookingHistory = (from d in _dbContext.Bookings
                                  join s in _dbContext.Schedules
                                  on d.ScheduleRecId equals s.ScheduleRecId
                                  orderby d.CreatedDate descending
                                  where d.EmailId == emailId
                                  select new Booking
                                  {
                                      BookingRecId = d.BookingRecId,
                                      CreatedDate = d.CreatedDate,
                                      DateOfJourney = d.DateOfJourney,
                                      EmailId = d.EmailId,
                                      IsBusinessClass = d.IsBusinessClass,
                                      IsCancelTicket = d.IsCancelTicket,
                                      IsRoundTrip = d.IsRoundTrip,
                                      Meal = d.Meal,
                                      Name = d.Name,
                                      NoOfSeats = d.NoOfSeats,
                                      Passengers = d.Passengers,
                                      PNRNumber = d.PNRNumber,
                                      ReturnDateOfJourney = d.ReturnDateOfJourney,
                                      ReturnIsBusinessClass = d.ReturnIsBusinessClass,
                                      ReturnScheduleRecId = d.ReturnScheduleRecId,
                                      ScheduleRecId = d.ScheduleRecId,
                                      Schedules = d.Schedules,
                                      Airlines = s.Airlines,
                                      TicketCost = d.TicketCost
                                  });

            return bookingHistory;
        }
    }
}
