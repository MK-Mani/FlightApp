using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedServices.Models
{
    public class Passenger
    {
        [Key]
        public int PassengerRecId { get; set; }

        public virtual Booking Bookings { get; set; }

        [ForeignKey("Bookings")]
        public int BookingRecId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int Age { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(10)]
        public string SeatNumbers { get; set; }

        [MaxLength(10)]
        public string ReturnSeatNumbers { get; set; }
    }
}
