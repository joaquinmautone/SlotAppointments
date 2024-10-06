using Microsoft.Extensions.Options;
using SlotAppointments.ServiceAgents.Availability.Configuration;
using SlotAppointments.ServiceAgents.Availability.Dtos;
using System.Text.Json;

namespace SlotAppointments.ServiceAgents.Availability
{
    public class AvailabilityServiceAgent : ServiceAgentBase, IAvailabilityServiceAgent
    {
        const string DATE_FORMAT = "yyyyMMdd";
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new AvailabilityJsonConverter() },
            PropertyNameCaseInsensitive = true
        };

        public AvailabilityServiceAgent(HttpClient httpClient, IOptions<ApiSettings> apiSettings) : base(httpClient, apiSettings)
        {            
        }

        public async Task<AvailabilityResponse> GetWeeklyAvailabilityAsync(DateTime dayOfWeek)
        {
            try
            {
                AvailabilityDto availability = await GetAsync<AvailabilityDto>($"/availability/GetWeeklyAvailability/{dayOfWeek.ToString(DATE_FORMAT)}", options);

                return new AvailabilityResponse(true, null, availability);
            }
            catch (HttpRequestException ex)
            {
                return new AvailabilityResponse(false, ex.Message);
            }
        }

        public async Task<TakeSlotResponse> TakeSlot(TakeSlotRequest slotRequest)
        {
            try
            {
                await PostAsync($"/availability/TakeSlot", slotRequest);

                return new TakeSlotResponse(true, null);
            }
            catch (HttpRequestException ex)
            {
                return new TakeSlotResponse(false, ex.Message);
            }
        }
    }
}
