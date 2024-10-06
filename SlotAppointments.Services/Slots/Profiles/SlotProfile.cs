using AutoMapper;

namespace SlotAppointments.Services.Slots.Profiles
{
    public class SlotProfile : Profile
    {
        public SlotProfile()
        {
            CreateMap<Domain.Slots.Entities.Slot, SlotReadingDto>();
            CreateMap<ServiceAgents.Availability.Dtos.SlotDto, Domain.Slots.Entities.Slot>();
            CreateMap<ServiceAgents.Availability.Dtos.SlotDto, SlotReadingDto>();
            CreateMap<SlotCreationDto, ServiceAgents.Availability.Dtos.TakeSlotRequest>();
            CreateMap<PatientCreationDto, ServiceAgents.Availability.Dtos.PatientDto>();
        }
    }
}
