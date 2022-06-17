using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedServices.Models
{
    public class Airline
    {
        [Key]
        public int AirlineRecId { get; set; }

        [MaxLength(50)]
        public string AirlineName { get; set; }

        [MaxLength(100)]
        public string AirlineLogo { get; set; }

        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [MaxLength(200)]
        public string ContactAddress { get; set; }

        [MaxLength(20)]
        public string DiscountCode { get; set; }

        public double? DiscountAmount { get; set; }

        public bool IsBlock { get; set; }
        
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
