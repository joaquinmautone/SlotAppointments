namespace SlotAppointments.ServiceAgents.Availability.Dtos
{
    public class DayDto
    {
        public WorkPeriodDto WorkPeriod { get; set; }
        public List<SlotDto> BusySlots { get; set; }
    }
}
