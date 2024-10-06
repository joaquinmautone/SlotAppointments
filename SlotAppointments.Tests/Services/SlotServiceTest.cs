using AutoMapper;
using Moq;
using SlotAppointments.ServiceAgents.Availability;
using SlotAppointments.ServiceAgents.Availability.Dtos;
using SlotAppointments.Services.Slots;
using SlotAppointments.Services.Slots.Profiles;

namespace SlotAppointments.Tests.Services
{
    public class SlotServiceTests
    {
        private readonly SlotService _slotService;
        private readonly Mock<IAvailabilityServiceAgent> _availabilitySAMock;
        private readonly IMapper _mapper;

        public SlotServiceTests()
        {
            _availabilitySAMock = new Mock<IAvailabilityServiceAgent>();
            _mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SlotProfile());
                mc.AddProfile(new AvailabilityProfile());
            }).CreateMapper();
            _slotService = new SlotService(_availabilitySAMock.Object, _mapper);
        }

        [Fact]
        public async Task GetWeeklyAvailability_ShouldReturnSlots_WhenResponseIsSuccess()
        {
            // Arrange
            var dayOfWeek = new DateTime(2024, 09, 30);
            var availability = new AvailabilityDto
            {
                Facility = new FacilityDto(),
                SlotDurationMinutes = 30,
                Days = new Dictionary<string, DayDto>
                {
                    ["MONDAY"] = new DayDto
                    {
                        WorkPeriod = new WorkPeriodDto
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<SlotDto>()
                    }
                }
            };
            var availabilityResponse = new AvailabilityResponse(true, null, availability);
            
            _availabilitySAMock.Setup(sa => sa.GetWeeklyAvailabilityAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(availabilityResponse);

            // Act
            var result = await _slotService.GetWeeklyAvailability(dayOfWeek);

            // Assert
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetWeeklyAvailability_ShouldReturnEmptySlots_WhenResponseFails()
        {
            // Arrange
            var dayOfWeek = new DateTime(2024, 09, 30);
            var availability = new AvailabilityDto
            {
                Facility = new FacilityDto(),
                SlotDurationMinutes = 30,
                Days = new Dictionary<string, DayDto>
                {
                    ["MONDAY"] = new DayDto
                    {
                        WorkPeriod = new WorkPeriodDto
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<SlotDto>()
                    }
                }
            };
            var availabilityResponse = new AvailabilityResponse(false, null, availability);

            _availabilitySAMock.Setup(sa => sa.GetWeeklyAvailabilityAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(availabilityResponse);

            // Act
            var result = await _slotService.GetWeeklyAvailability(dayOfWeek);

            // Assert
            Assert.False(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task AddSlotAsync_ShouldReturnTrue_WhenResponseIsSuccess()
        {
            // Arrange
            var slotCreationDto = new SlotCreationDto { Start = DateTime.Now, End = DateTime.Now.AddHours(1) };
            var takeSlotResponse = new TakeSlotResponse(true, null);
            var slotReadingDto = new SlotReadingDto { Start = slotCreationDto.Start, End = slotCreationDto.End };
            var slotRequest = new TakeSlotRequest();
            _availabilitySAMock.Setup(sa => sa.TakeSlot(It.IsAny<TakeSlotRequest>())).ReturnsAsync(takeSlotResponse);

            // Act
            var result = await _slotService.AddSlotAsync(slotCreationDto);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task AddSlotAsync_ShouldReturnError_WhenResponseFails()
        {
            // Arrange
            var slotCreationDto = new SlotCreationDto { Start = DateTime.Now, End = DateTime.Now.AddHours(1) };
            var takeSlotResponse = new TakeSlotResponse(false, null);
            var slotReadingDto = new SlotReadingDto { Start = slotCreationDto.Start, End = slotCreationDto.End };
            var slotRequest = new TakeSlotRequest();
            _availabilitySAMock.Setup(sa => sa.TakeSlot(It.IsAny<TakeSlotRequest>())).ReturnsAsync(takeSlotResponse);

            // Act
            var result = await _slotService.AddSlotAsync(slotCreationDto);

            // Assert
            Assert.False(result.Success);
        }
    }
}