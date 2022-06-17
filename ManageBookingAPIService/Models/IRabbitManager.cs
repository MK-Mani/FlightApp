namespace ManageBookingAPIService.Models
{
    public interface IRabbitManager
    {
        void Publish<T>(T message, string routeKey) where T : class;
    }
}
