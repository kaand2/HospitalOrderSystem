using HospitalOrderSystem.Application.DTOs.Auth;
using HospitalOrderSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalOrderSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result is null)
            {
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });
            }

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("bootstrap-admin")]
        public async Task<IActionResult> CreateInitialAdmin(
            CreateInitialAdminDto dto)
        {
            var result =
                await _authService.CreateInitialAdminAsync(dto);

            return Created("", result);
        }
    }
}

