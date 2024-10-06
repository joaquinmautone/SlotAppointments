using Microsoft.AspNetCore.Mvc;
using SlotAppointments.Services.Communication;
using SlotAppointments.Services.Slots;

namespace SlotAppointments.Controllers
{
    [ApiController]
    [Route("api/slots")]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;
        private readonly ILogger<SlotController> _logger;

        public SlotController(ISlotService slotService, ILogger<SlotController> logger)
        {
            this._slotService = slotService;
            _logger = logger;
        }

        [HttpGet("{dayOfWeek}", Name = "GetWeeklyAvailability")]
        public async Task<ActionResult<List<SlotReadingDto>>> Get(DateTime dayOfWeek)
        {
            try
            {
                ApiResponse<List<SlotReadingDto>> response = await _slotService.GetWeeklyAvailability(dayOfWeek);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the slot");
                return StatusCode(500, "Error retrieving GetWeeklyAvailabiliy");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SlotReadingDto>> Post([FromBody] SlotCreationDto slot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _slotService.AddSlotAsync(slot);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the slot");
                return StatusCode(500, "Error adding the slot");
            }            
        }
    }
}
