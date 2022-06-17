using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageSchedulesAPIService.Models
{
    public class SQLScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLScheduleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Schedule AddSchedule(Schedule flightSchedules)
        {
            _dbContext.Schedules.Add(flightSchedules);
            _dbContext.SaveChanges();
            return flightSchedules;
        }

        public IEnumerable<Schedule> GetSchedulesByFilter(Schedule searchFilter)
        {
            var flightDetails = (from d in _dbContext.Schedules
                                 join a in _dbContext.Airlines
                                 on d.AirlineRecId equals a.AirlineRecId
                                where (!string.IsNullOrEmpty(searchFilter.FromPlace) && d.FromPlace.ToLower() == searchFilter.FromPlace.ToLower())
                                && (!string.IsNullOrEmpty(searchFilter.ToPlace) && d.ToPlace.ToLower() == searchFilter.ToPlace.ToLower())
                                && ((searchFilter.StartDateTime != DateTime.MinValue && d.StartDateTime >= searchFilter.StartDateTime)
                                || (searchFilter.EndDateTime != DateTime.MinValue && d.StartDateTime <= searchFilter.EndDateTime)
                                && (d.NoOfBussinessClsSeats > 0 || d.NoOfNonBussinessClsSeats > 0))
                        select new Schedule
                        {
                            ScheduleRecId = d.ScheduleRecId,
                            Airlines = d.Airlines,
                            AirlineRecId = d.AirlineRecId,
                            FlightNumber = d.FlightNumber,
                            FromPlace = d.FromPlace,
                            ToPlace = d.ToPlace,
                            StartDateTime = d.StartDateTime,
                            EndDateTime = d.EndDateTime,
                            ScheduleDays = d.ScheduleDays,
                            InstrumentUsed = d.InstrumentUsed,
                            NoOfBussinessClsSeats = d.NoOfBussinessClsSeats,
                            NoOfNonBussinessClsSeats = d.NoOfNonBussinessClsSeats,
                            BusinessClassTicket = d.BusinessClassTicket,
                            NonBusinessClassTicket = d.NonBusinessClassTicket,
                            NoOfRows = d.NoOfRows,
                            Meal = d.Meal
                        });

            return flightDetails;
        }

        public void UpdateSeats(RabbitMqTicket ticketBookingDetails)
        {
            var scheduleDetails = _dbContext.Schedules.Find(ticketBookingDetails.ScheduleRecId);

            if (scheduleDetails != null)
            {
                var seats = ticketBookingDetails.NoOfSeats;

                if (ticketBookingDetails.IsBusinessClass)
                {
                    scheduleDetails.NoOfBussinessClsSeats = ticketBookingDetails.IsBooking ?
                        (scheduleDetails.NoOfBussinessClsSeats - seats) : (scheduleDetails.NoOfBussinessClsSeats + seats);
                }
                else
                {
                    scheduleDetails.NoOfNonBussinessClsSeats = ticketBookingDetails.IsBooking ?
                        (scheduleDetails.NoOfNonBussinessClsSeats - seats) : (scheduleDetails.NoOfNonBussinessClsSeats + seats);
                }

                // For Round Trip
                if(ticketBookingDetails.IsRoundTrip)
                {
                    var returnSchedule = _dbContext.Schedules.Find(ticketBookingDetails.ReturnScheduleRecId);

                    if(returnSchedule != null)
                    {
                        if (ticketBookingDetails.IsBusinessClass)
                        {
                            returnSchedule.NoOfBussinessClsSeats = ticketBookingDetails.IsBooking ?
                                (returnSchedule.NoOfBussinessClsSeats - seats) : (returnSchedule.NoOfBussinessClsSeats + seats);
                        }
                        else
                        {
                            returnSchedule.NoOfNonBussinessClsSeats = ticketBookingDetails.IsBooking ?
                                (returnSchedule.NoOfNonBussinessClsSeats - seats) : (returnSchedule.NoOfNonBussinessClsSeats + seats);
                        }
                    }
                }

                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<Schedule> GetAllSchedules()
        {
            var schedules = (from d in _dbContext.Schedules
                             select new Schedule
                             {
                                 ScheduleRecId = d.ScheduleRecId,
                                 Airlines = d.Airlines,
                                 AirlineRecId = d.AirlineRecId,
                                 FlightNumber = d.FlightNumber,
                                 FromPlace = d.FromPlace,
                                 ToPlace = d.ToPlace,
                                 StartDateTime = d.StartDateTime ,
                                 EndDateTime = d.EndDateTime,
                                 ScheduleDays = d.ScheduleDays,
                                InstrumentUsed = d.InstrumentUsed,
                                 NoOfBussinessClsSeats = d.NoOfBussinessClsSeats,
                                 NoOfNonBussinessClsSeats = d.NoOfNonBussinessClsSeats,
                                 BusinessClassTicket = d.BusinessClassTicket,
                                 NonBusinessClassTicket = d.NonBusinessClassTicket,
                                 NoOfRows = d.NoOfRows,
                                 Meal = d.Meal
                             });
            return schedules;
        }
    }
}
