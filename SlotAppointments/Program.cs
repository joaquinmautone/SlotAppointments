using Microsoft.Extensions.Options;
using SlotAppointments.ServiceAgents.Availability;
using SlotAppointments.ServiceAgents.Availability.Configuration;
using SlotAppointments.Services.Slots;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddHttpClient<IAvailabilityServiceAgent, AvailabilityServiceAgent>()
    .ConfigureHttpClient((sp, client) =>
    {
        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        client.BaseAddress = new Uri(settings.BaseUrl);
    });
builder.Services.AddScoped<ISlotService, SlotService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
