using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedServices.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleRecId { get; set; }

        [ForeignKey("Airlines")]
        public int AirlineRecId { get; set; }
        public virtual Airline Airlines { get; set; }

        [MaxLength(25)]
        public string FlightNumber { get; set; }

        [MaxLength(50)]
        public string FromPlace { get; set; }

        [MaxLength(50)]
        public string ToPlace { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        [MaxLength(100)]
        public string ScheduleDays { get; set; }

        [MaxLength(50)]
        public string InstrumentUsed { get; set; }

        public int NoOfBussinessClsSeats { get; set; }

        public int NoOfNonBussinessClsSeats { get; set; }

        public double BusinessClassTicket { get; set; }

        public double NonBusinessClassTicket { get; set; }

        public int NoOfRows { get; set; }

        [MaxLength(10)]
        public string Meal { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
