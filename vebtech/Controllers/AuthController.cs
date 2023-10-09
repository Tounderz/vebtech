using Microsoft.AspNetCore.Mvc;
using System.Net;
using vebtech.CustomException;
using vebtech.Models;
using vebtech.Models.DTO;
using vebtech.Repositories.Abstract;

namespace vebtech.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<UsersController> _logger;

    public AuthController(IAuthRepository authRepository, ILogger<UsersController> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    [ProducesResponseType(typeof(Admin), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("/signIn")]
    public async Task<IActionResult> SignIn([FromForm] AdminDto adminDto)
    {
        try
        {
            var encodedJwt = await _authRepository.GenerateJwt(adminDto) ??
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid email or password");

            var response = new
            {
                access_token = encodedJwt,
                username = adminDto.Email
            };

            return Ok(response);
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"SignIn method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }
    }

    [ProducesResponseType(typeof(Admin), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("/signUp")]
    public async Task<IActionResult> SignUp([FromForm] AdminDto adminDto)
    {
        try
        {
            if (await _authRepository.IsExistEmail(adminDto.Email))
            {
                throw new HttpResponseException(HttpStatusCode.Conflict, "Admin with this email exist");
            }

            var admin = await _authRepository.SignUp(adminDto);
            return Ok(new { message = "successfully" });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"SignUp method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }
    }
}