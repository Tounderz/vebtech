using Microsoft.AspNetCore.Mvc;
using System.Net;
using vebtech.CustomException;
using vebtech.Application.Services.Interfaces;
using vebtech.Domain.Models.DTO;
using vebtech.Utils.Interfaces;

namespace vebtech.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidateUtils _validateUtils;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService,
        ILogger<AuthController> logger,
        IJwtService jwtService,
        IValidateUtils validateUtils)
    {
        _authService = authService;
        _logger = logger;
        _jwtService = jwtService;
        _validateUtils = validateUtils;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("/signIn")]
    public async Task<IActionResult> SignIn([FromForm] AdminDto adminDto)
    {
        try
        {
           _validateUtils.ValidateAdmin(adminDto);
            var admin = await _authService.SignIn(adminDto) ?? throw new HttpResponseException(HttpStatusCode.NotFound, "Invalid email or password");
            var encodedJwt = _jwtService.GenerateJwt(admin.Email);
            var response = new
            {
                access_token = encodedJwt,
                username = admin.Email
            };

            return Ok(response);
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.StatusCode);
            return BadRequest(new { message = ex.Value });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.HResult);
            return BadRequest(new { message = "Error" });
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("/signUp")]
    public async Task<IActionResult> SignUp([FromForm] AdminDto adminDto)
    {
        try
        {
            _validateUtils.ValidateAdmin(adminDto);
            if (await _authService.IsExistEmail(adminDto.Email))
            {
                throw new HttpResponseException(HttpStatusCode.Conflict, "Admin with this email exist");
            }

            var admin = await _authService.SignUp(adminDto);
            return Ok(new { message = "successfully" });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.StatusCode);
            return BadRequest(new { message = ex.Value });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.HResult);
            return BadRequest(new { message = "Error" });
        }
    }
}
