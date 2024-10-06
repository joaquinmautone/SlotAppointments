namespace SlotAppointments.ServiceAgents.Availability.Dtos
{
    public class AvailabilityDto
    {
        public FacilityDto Facility { get; set; }
        public int SlotDurationMinutes { get; set; }
        public Dictionary<string, DayDto> Days { get; set; } = new Dictionary<string, DayDto>();
    }
}