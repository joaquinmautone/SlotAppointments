
using AutoMapper;
using SlotAppointments.Domain.Availability.Entities;
using SlotAppointments.ServiceAgents.Availability;
using SlotAppointments.ServiceAgents.Availability.Dtos;
using SlotAppointments.Services.Communication;

namespace SlotAppointments.Services.Slots
{
    public class SlotService : ISlotService
    {
        private IAvailabilityServiceAgent _availabilitySA;
        private readonly IMapper _mapper;

        public SlotService(IAvailabilityServiceAgent availabilitySA, IMapper mapper)
        {
            _availabilitySA = availabilitySA;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<SlotReadingDto>>> GetWeeklyAvailability(DateTime dayOfWeek)
        {
            List<SlotReadingDto> slots = new List<SlotReadingDto>();
            AvailabilityResponse availabilityResponse = await _availabilitySA.GetWeeklyAvailabilityAsync(dayOfWeek);

            if (availabilityResponse.Success)
            {
                Availability availability = _mapper.Map<Availability>(availabilityResponse.Availability);
                slots = _mapper.Map<List<SlotReadingDto>>(availability.GetWeeklyAvailableSlots(dayOfWeek));
            }

            ApiResponse<List<SlotReadingDto>> response = new ApiResponse<List<SlotReadingDto>>(
                availabilityResponse.Success, 
                availabilityResponse.Message,
                slots);

            return response;
        }

        public async Task<ApiResponse<SlotReadingDto>> AddSlotAsync(SlotCreationDto slot)
        {
            TakeSlotRequest slotRequest = _mapper.Map<TakeSlotRequest>(slot);
            TakeSlotResponse takeSlotResponse = await _availabilitySA.TakeSlot(slotRequest);
            ApiResponse<SlotReadingDto> response = new ApiResponse<SlotReadingDto>(
                takeSlotResponse.Success,
                takeSlotResponse.Message,
                _mapper.Map<SlotReadingDto>(takeSlotResponse.Slot));

            return response;
        }
    }
}
