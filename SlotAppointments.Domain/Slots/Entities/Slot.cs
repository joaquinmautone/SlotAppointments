namespace SlotAppointments.Domain.Slots.Entities
{
    public class Slot
    {
        public Guid FacilityId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool Overlaps(Slot slot)
        {
            bool overlaps = slot.Start <= this.Start && slot.End > this.Start;
            overlaps = overlaps || slot.Start >= this.Start && slot.Start < this.End;

            return overlaps;
        }
    }
}