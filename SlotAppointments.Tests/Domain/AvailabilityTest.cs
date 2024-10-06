using SlotAppointments.Domain.Availability.Entities;
using SlotAppointments.Domain.Slots.Entities;

namespace SlotAppointments.Tests.Domain
{
    public class AvailabilityTests
    {
        [Fact]
        public void GetWeeklyAvailableSlots_ShouldReturnAvailableSlots_WhenWorkPeriodExists()
        {
            // Arrange
            var monday = new DateTime(2024, 09, 30); 
            var availability = new Availability
            {
                Facility = new Facility(),
                SlotDurationMinutes = 30,
                Days = new Dictionary<string, Day>
                {
                    ["MONDAY"] = new Day
                    {
                        WorkPeriod = new WorkPeriod
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<Slot>()
                    }
                }
            };

            // Act
            var availableSlots = availability.GetWeeklyAvailableSlots(monday);

            // Assert
            Assert.NotEmpty(availableSlots);
            Assert.True(availableSlots.All(slot => slot.Start.TimeOfDay >= new TimeSpan(9, 0, 0)
                && slot.End.TimeOfDay <= new TimeSpan(17, 0, 0)));
        }

        [Fact]
        public void GetWeeklyAvailableSlots_ShouldSkipLunchBreak_WhenWorkPeriodHasLunch()
        {
            // Arrange
            var monday = new DateTime(2024, 09, 30);
            int lunchStartHour = 12;
            int lunchEndHour = 13;
            var availability = new Availability
            {
                Facility = new Facility(),
                SlotDurationMinutes = 30,
                Days = new Dictionary<string, Day>
                {
                    ["TUESDAY"] = new Day
                    {
                        WorkPeriod = new WorkPeriod
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = lunchStartHour,
                            LunchEndHour = lunchEndHour
                        },
                        BusySlots = new List<Slot>()
                    }
                }
            };

            // Act
            var availableSlots = availability.GetWeeklyAvailableSlots(monday);

            // Assert
            Assert.DoesNotContain(availableSlots, slot => slot.Start.Hour >= lunchStartHour && slot.Start.Hour < lunchEndHour);
        }

        [Fact]
        public void GetWeeklyAvailableSlots_ShouldExcludeBusySlots_WhenTheyOverlap()
        {
            // Arrange
            var monday = new DateTime(2024, 09, 30);
            DateTime busySlotStart = new DateTime(2024, 10, 2, 10, 0, 0);
            var availability = new Availability
            {
                Facility = new Facility(),
                SlotDurationMinutes = 30,
                Days = new Dictionary<string, Day>
                {
                    ["WEDNESDAY"] = new Day
                    {
                        WorkPeriod = new WorkPeriod
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<Slot>
                        {
                            new Slot
                            {
                                Start = busySlotStart,
                                End = new DateTime(2024, 10, 2, 10, 30, 0)
                            }
                        }
                    }
                }
            };

            // Act
            var availableSlots = availability.GetWeeklyAvailableSlots(monday);

            // Assert
            Assert.DoesNotContain(availableSlots, slot => slot.Start == busySlotStart);
        }

        [Fact]
        public void GetWeeklyAvailableSlots_ShouldReturnEmpty_WhenNoWorkPeriodExists()
        {
            // Arrange
            var monday = new DateTime(2024, 09, 30);
            var availability = new Availability
            {
                Facility = new Facility(),
                SlotDurationMinutes = 30,
                Days = new Dictionary<string, Day>
                {
                    ["THURSDAY"] = new Day
                    {
                        WorkPeriod = null,
                        BusySlots = new List<Slot>()
                    }
                }
            };

            // Act
            var availableSlots = availability.GetWeeklyAvailableSlots(monday);

            // Assert
            Assert.Empty(availableSlots);
        }

        [Fact]
        public void GetWeeklyAvailableSlots_ShouldReturnEmpty_WhenNoSlotDurationMinutesExists()
        {
            // Arrange
            var monday = new DateTime(2024, 09, 30);
            var availability = new Availability
            {
                Facility = new Facility(),
                Days = new Dictionary<string, Day>
                {
                    ["THURSDAY"] = new Day
                    {
                        WorkPeriod = new WorkPeriod
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<Slot>()
                    }
                }
            };

            // Act
            var availableSlots = availability.GetWeeklyAvailableSlots(monday);

            // Assert
            Assert.Empty(availableSlots);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(45)]
        public void GetWeeklyAvailableSlots_ShouldGenerateSlotsWithVaryingDurations(int slotDurationMinutes)
        {
            // Arrange
            var monday = new DateTime(2024, 09, 30);
            var availability = new Availability
            {
                Facility = new Facility(),
                SlotDurationMinutes = slotDurationMinutes,
                Days = new Dictionary<string, Day>
                {
                    ["FRIDAY"] = new Day
                    {
                        WorkPeriod = new WorkPeriod
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<Slot>()
                    }
                }
            };

            // Act
            var availableSlots = availability.GetWeeklyAvailableSlots(monday);

            // Assert
            Assert.All(availableSlots, slot => Assert.Equal(slotDurationMinutes, (slot.End - slot.Start).TotalMinutes));
        }

        [Fact]
        public void GetWeeklyAvailableSlots_ShouldHandleMultipleDaysWithBusySlots()
        {
            // Arrange
            var monday = new DateTime(2024, 09, 30);
            var availability = new Availability
            {
                Facility = new Facility(),
                SlotDurationMinutes = 30,
                Days = new Dictionary<string, Day>
                {
                    ["MONDAY"] = new Day
                    {
                        WorkPeriod = new WorkPeriod
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<Slot>
                        {
                            new Slot
                            {
                                Start = new DateTime(2024, 09, 30, 13, 0, 0),
                                End = new DateTime(2024, 09, 30, 13, 30, 0)
                            }
                        }
                    },
                    ["SATURDAY"] = new Day
                    {
                        WorkPeriod = new WorkPeriod
                        {
                            StartHour = 9,
                            EndHour = 17,
                            LunchStartHour = 12,
                            LunchEndHour = 13
                        },
                        BusySlots = new List<Slot>
                        {
                            new Slot
                            {
                                Start = new DateTime(2024, 10, 05, 16, 30, 0),
                                End = new DateTime(2024, 10, 05, 17, 0, 0)
                            }
                        }
                    }
                }
            };

            // Act
            var availableSlots = availability.GetWeeklyAvailableSlots(monday);

            // Assert
            Assert.True(availableSlots.All(slot => 
                slot.Start >= new DateTime(2024, 09, 30, 09, 0, 0) && 
                slot.End <= new DateTime(2024, 10, 05, 17, 0, 0)));
            // Monday
            Assert.DoesNotContain(availableSlots, slot => slot.Start == new DateTime(2024, 09, 30, 13, 0, 0));
            // Saturday
            Assert.DoesNotContain(availableSlots, slot => slot.Start == new DateTime(2024, 10, 05, 16, 30, 0));
        }
    }
}
