namespace SharedServices.Models
{
    public class RabbitMqTicket
    {
        public int ScheduleRecId { get; set; }

        public int NoOfSeats { get; set; }

        public bool IsBooking { get; set; }

        public bool IsBusinessClass { get; set; }

        public bool IsRoundTrip { get; set; }

        public int? ReturnScheduleRecId { get; set; }
    }
}
