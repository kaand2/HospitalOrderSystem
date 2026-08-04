using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Patients;
using HospitalOrderSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalOrderSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IValidator<CreatePatientDto> _createPatientValidator;
        private readonly IValidator<UpdatePatientDto> _updatePatientValidator;
        public PatientsController(IPatientService patientService,
            IValidator<CreatePatientDto> createPatientValidator,
            IValidator<UpdatePatientDto> updatePatientValidator)
        {
            _patientService = patientService;
            _createPatientValidator = createPatientValidator;
            _updatePatientValidator = updatePatientValidator;
        }
        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetAll()
        {
            List<PatientDto> patients =
                await _patientService.GetAllAsync();

            return Ok(patients);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PatientDto>> GetById(int id)
        {
            try
            {
                PatientDto patient =
                    await _patientService.GetByIdAsync(id);
                return Ok(patient);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new
                {
                    message = exception.Message
                });
            }
        }
        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create(
            [FromBody] CreatePatientDto createPatientDto)
        {
            var validationResult =
                await _createPatientValidator.ValidateAsync(createPatientDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .ToArray());

                return BadRequest(new
                {
                    errors
                });
            }
            try
            {
                PatientDto createdPatient =
                    await _patientService.CreateAsync(createPatientDto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdPatient.Id },
                    createdPatient);
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new
                {
                    message = exception.Message
                });
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PatientDto>> Update(
            int id,
            [FromBody] UpdatePatientDto updatePatientDto)
        {
            var validationResult =
                await _updatePatientValidator.ValidateAsync(updatePatientDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .ToArray());

                return BadRequest(new
                {
                    errors
                });
            }
            try
            {
                PatientDto updatedPatient =
                    await _patientService.UpdateAsync(
                        id,
                        updatePatientDto);

                return Ok(updatedPatient);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new
                {
                    message = exception.Message
                });
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new
                {
                    message = exception.Message
                });
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _patientService.DeleteAsync(id);

                return Ok(new
                {
                    message = "Hasta silindi."
                });
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new
                {
                    message = exception.Message
                });
            }
        }
    }
}
