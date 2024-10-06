
using SlotAppointments.Domain.Slots.Entities;

namespace SlotAppointments.Domain.Availability.Entities
{
    public class Availability
    {
        public Facility Facility { get; set; }
        public int SlotDurationMinutes { get; set; }
        public Dictionary<string, Day> Days { get; set; } = new Dictionary<string, Day>();

        public List<Slot> GetWeeklyAvailableSlots(DateTime datePeriod)
        {
            var availableSlots = new List<Slot>();
            string[] dayNames = {"MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY"};

            if (SlotDurationMinutes > 0)
            {
                foreach (var day in this.Days)
                {
                    var workPeriod = day.Value.WorkPeriod;

                    if (workPeriod == null)
                        continue;

                    int dayOfWeek = Array.IndexOf(dayNames, day.Key.ToUpper());
                    DateTime workStart = new DateTime(datePeriod.Year, datePeriod.Month, datePeriod.Day, workPeriod.StartHour, 0, 0);
                    workStart = workStart.AddDays(dayOfWeek);
                    DateTime workEnd = new DateTime(workStart.Year, workStart.Month, workStart.Day, workPeriod.EndHour, 0, 0);
                    DateTime lunchStart = new DateTime(workStart.Year, workStart.Month, workStart.Day, workPeriod.LunchStartHour, 0, 0);
                    DateTime lunchEnd = new DateTime(workStart.Year, workStart.Month, workStart.Day, workPeriod.LunchEndHour, 0, 0);
                    var busySlots = day.Value.BusySlots.OrderBy(s => s.Start).ToList();

                    while (workStart < workEnd)
                    {
                        if (workStart >= lunchStart && workStart < lunchEnd)
                        {
                            workStart = lunchEnd;
                        }

                        Slot slot = new Slot()
                        {
                            FacilityId = Facility.Id,
                            Start = workStart,
                            End = workStart.AddMinutes(this.SlotDurationMinutes)
                        };

                        if (!busySlots.Exists(bs => bs.Overlaps(slot)))
                        {
                            availableSlots.Add(slot);
                        }

                        workStart = workStart.AddMinutes(this.SlotDurationMinutes);
                    }
                }
            }
            
            return availableSlots;
        }
    }
}