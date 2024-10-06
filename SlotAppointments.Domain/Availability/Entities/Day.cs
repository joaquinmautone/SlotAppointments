using SlotAppointments.Domain.Slots.Entities;

namespace SlotAppointments.Domain.Availability.Entities
{
    public class Day
    {
        public WorkPeriod WorkPeriod { get; set; }
        public List<Slot> BusySlots { get; set; }
    }
}
