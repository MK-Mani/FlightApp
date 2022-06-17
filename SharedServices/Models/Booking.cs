using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedServices.Models
{
    public class Booking
    {
        [Key]
        public int BookingRecId { get; set; }

        public virtual Schedule Schedules { get; set; }

        [ForeignKey("Schedules")]
        public int ScheduleRecId { get; set; }       

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string EmailId { get; set; }

        public int NoOfSeats { get; set; }

        [MaxLength(20)]
        public string Meal { get; set; }

        public int PNRNumber { get; set; }

        public DateTime DateOfJourney { get; set; }

        public bool IsBusinessClass { get; set; }

        public bool IsRoundTrip { get; set; }

        public int? ReturnScheduleRecId { get; set; }

        public DateTime? ReturnDateOfJourney { get; set; }

        public bool ReturnIsBusinessClass { get; set; }

        public bool IsCancelTicket { get; set; }

        public DateTime CreatedDate { get; set; }

        public double TicketCost { get; set; }
                
        [NotMapped]
        public Airline Airlines { get; set; }

        public virtual ICollection<Passenger> Passengers { get; set; }
    }
}
