namespace SlotAppointments.Services.Slots
{
    public class SlotCreationDto
    {
        public Guid FacilityId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }
        public PatientCreationDto Patient { get; set; }
    }
}