using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var response = _authService.RegisterUserService(registerDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPut("EditUser")]
        [Authorize]
        public IActionResult UpdateUser(UpdateUserDto contactDto)
        {
            var contact = new UpdateUserDto()
            {
                userId = contactDto.userId,
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                ContactNumber = contactDto.ContactNumber,
                Email = contactDto.Email,
                ProfilePic = contactDto.ProfilePic,
                ImageByte = contactDto.ImageByte,
                LoginId = contactDto.LoginId,

            };

            var response = _authService.ModifyUser(contact);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("Login")]
        public IActionResult login(LoginDto loginDto)
        {
            var response = _authService.LoginUserService(loginDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
        [HttpPut("AddNewPassword")]
        public IActionResult AddNewPassword(NewPasswordDto newPassword)
        {
            var response = _authService.AddNewPassword(newPassword);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
        [HttpPost("ValidateForgotPassword")]
        public IActionResult ValidateForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var response = _authService.ValidateUserForForgetPassword(forgotPasswordDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpGet("GetUserByUserName/{userName}")]
        public IActionResult GetUserByUserName(string userName)
        {
            var response = _authService.GetUserByUserName(userName);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
    }
}
