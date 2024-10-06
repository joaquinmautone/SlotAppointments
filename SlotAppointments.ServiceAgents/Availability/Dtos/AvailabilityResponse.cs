namespace SlotAppointments.ServiceAgents.Availability.Dtos
{
    public class AvailabilityResponse: BaseResponse
    {
        public AvailabilityResponse(bool success, string? message, AvailabilityDto? availability = null) : base(success, message)
        {
            Availability = availability;
        }

        public AvailabilityDto? Availability { get; set; }
    }
}
