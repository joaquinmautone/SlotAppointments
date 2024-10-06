namespace SlotAppointments.Services.Slots
{
    public class SlotReadingDto
    {
        public Guid FacilityId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}