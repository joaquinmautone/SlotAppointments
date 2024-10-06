namespace SlotAppointments.ServiceAgents.Availability.Dtos
{
    public class TakeSlotRequest
    {
        public Guid FacilityId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }
        public PatientDto Patient { get; set; }
    }
}
