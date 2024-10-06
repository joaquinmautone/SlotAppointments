using AutoMapper;

namespace SlotAppointments.Services.Slots.Profiles
{
    public class AvailabilityProfile : Profile
    {
        public AvailabilityProfile()
        {
            CreateMap<ServiceAgents.Availability.Dtos.AvailabilityDto, Domain.Availability.Entities.Availability>();
            CreateMap<ServiceAgents.Availability.Dtos.FacilityDto, Domain.Availability.Entities.Facility>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FacilityId));
            CreateMap<ServiceAgents.Availability.Dtos.DayDto, Domain.Availability.Entities.Day>();
            CreateMap<ServiceAgents.Availability.Dtos.WorkPeriodDto, Domain.Availability.Entities.WorkPeriod>();
            CreateMap<ServiceAgents.Availability.Dtos.SlotDto, Domain.Slots.Entities.Slot>();
        }
    }
}
