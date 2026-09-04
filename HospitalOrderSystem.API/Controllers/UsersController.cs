using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Users;
using HospitalOrderSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalOrderSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;
        public UsersController(IUserService userService,
            IValidator<CreateUserDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator)
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll([FromQuery] int? role = null)
        {
            List<UserDto> users =
                await _userService.GetAllAsync(role);

            return Ok(users);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            UserDto user =
                await _userService.GetByIdAsync(id);
            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<List<UserDto>>> Search(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName)
        {
            List<UserDto> users =
                await _userService.SearchAsync(firstName, lastName);
            return Ok(users);
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<List<UserDto>>> GetDoctors()
        {
            List<UserDto> doctors = await _userService.GetDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("doctors/search")]
        public async Task<ActionResult<List<UserDto>>> SearchDoctors(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName)
        {
            List<UserDto> doctors = await _userService.SearchDoctorsAsync(firstName, lastName);
            return Ok(doctors);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(
            [FromBody] CreateUserDto createUserDto)
        {
            var validationResult =
                await _createUserValidator.ValidateAsync(createUserDto);
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
            UserDto createdUser =
                await _userService.CreateAsync(createUserDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdUser.Id },
                createdUser);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDto>> Update(
            int id,
            [FromBody] UpdateUserDto updateUserDto)
        {
            var validationResult =
                await _updateUserValidator.ValidateAsync(updateUserDto);
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
            UserDto updatedUser =
                await _userService.UpdateAsync(
                    id,
                    updateUserDto);

            return Ok(updatedUser);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);

            return Ok(new
            {
                message = "Kullanıcı silindi."
            });
        }
    }
}
