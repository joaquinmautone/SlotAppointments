
using SlotAppointments.Services.Communication;

namespace SlotAppointments.Services.Slots
{
    public interface ISlotService
    {
        Task<ApiResponse<SlotReadingDto>> AddSlotAsync(SlotCreationDto slot);

        /// <summary>
        /// Retrieves the available slots for a specific week based on the given dayOfWeek
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns>SlotReadingDto list, which represent the available slots</returns>
        Task<ApiResponse<List<SlotReadingDto>>> GetWeeklyAvailability(DateTime dayOfWeek);
    }
}
