
using SlotAppointments.ServiceAgents.Availability.Dtos;

namespace SlotAppointments.ServiceAgents.Availability
{
    public interface IAvailabilityServiceAgent
    {
        Task<AvailabilityResponse> GetWeeklyAvailabilityAsync(DateTime dayOfWeek);
        Task<TakeSlotResponse> TakeSlot(TakeSlotRequest slotRequest);
    }
}
