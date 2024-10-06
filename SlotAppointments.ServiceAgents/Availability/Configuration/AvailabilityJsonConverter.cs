using SlotAppointments.ServiceAgents.Availability.Dtos;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlotAppointments.ServiceAgents.Availability.Configuration
{
    public class AvailabilityJsonConverter : JsonConverter<AvailabilityDto>
    {
        private static readonly HashSet<string> dayNames = new HashSet<string>
        {
            "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY"
        };

        public override AvailabilityDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var availability = new AvailabilityDto();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                string propertyName = reader.GetString();

                if (dayNames.Contains(propertyName?.ToUpper()))
                {
                    var daySchedule = JsonSerializer.Deserialize<DayDto>(ref reader);
                    availability.Days.Add(propertyName, daySchedule);
                }
                else
                {
                    var property = typeof(AvailabilityDto).GetProperty(propertyName);

                    if (property != null)
                    {
                        var value = JsonSerializer.Deserialize(ref reader, property.PropertyType);
                        property.SetValue(availability, value);
                    }
                }
            }

            return availability;
        }

        public override void Write(Utf8JsonWriter writer, AvailabilityDto value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            JsonSerializer.Serialize(writer, value, options);
            writer.WriteEndObject();
        }
    }
}
