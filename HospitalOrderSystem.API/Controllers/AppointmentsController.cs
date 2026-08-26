using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Appointments;
using HospitalOrderSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalOrderSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IValidator<CreateAppointmentDto> _createValidator;
        private readonly IValidator<UpdateAppointmentDto> _updateValidator;
        private readonly IValidator<CancelAppointmentDto> _cancelValidator;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IValidator<CreateAppointmentDto> createValidator,
            IValidator<UpdateAppointmentDto> updateValidator,
            IValidator<CancelAppointmentDto> cancelValidator)
        {
            _appointmentService = appointmentService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _cancelValidator = cancelValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppointmentDto>>> GetAll()
        {
            List<AppointmentDto> appointments = await _appointmentService.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            AppointmentDto appointment = await _appointmentService.GetByIdAsync(id);
            return Ok(appointment);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<AppointmentDto>>> Search(
            [FromQuery] string? patientName,
            [FromQuery] string? doctorName,
            [FromQuery] DateTime? date)
        {
            List<AppointmentDto> appointments =
                await _appointmentService.SearchAsync(patientName, doctorName, date);
            return Ok(appointments);
        }

        [HttpGet("available-time-slots")]
        public async Task<ActionResult<List<string>>> GetAvailableTimeSlots(
            [FromQuery] int doctorId,
            [FromQuery] DateTime date)
        {
            List<string> slots = await _appointmentService.GetAvailableTimeSlotsAsync(doctorId, date);
            return Ok(slots);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<AppointmentDto>> Create(
            [FromBody] CreateAppointmentDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(new { errors });
            }

            AppointmentDto created = await _appointmentService.CreateAsync(createDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<AppointmentDto>> Update(
            int id,
            [FromBody] UpdateAppointmentDto updateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(new { errors });
            }

            AppointmentDto updated = await _appointmentService.UpdateAsync(id, updateDto);
            return Ok(updated);
        }

        [HttpPut("{id:int}/cancel")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<AppointmentDto>> Cancel(
            int id,
            [FromBody] CancelAppointmentDto cancelDto)
        {
            var validationResult = await _cancelValidator.ValidateAsync(cancelDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(new { errors });
            }

            AppointmentDto cancelled = await _appointmentService.CancelAsync(id, cancelDto);
            return Ok(cancelled);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return Ok(new { message = "Randevu silindi." });
        }
    }
}
