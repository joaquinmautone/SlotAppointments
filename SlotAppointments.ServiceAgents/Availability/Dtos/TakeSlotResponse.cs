namespace SlotAppointments.ServiceAgents.Availability.Dtos
{
    public class TakeSlotResponse : BaseResponse
    {
        public TakeSlotResponse(bool success, string? message, SlotDto? slot = null) : base(success, message)
        {
            Slot = slot;
        }

        public SlotDto? Slot { get; set; }
    }
}
