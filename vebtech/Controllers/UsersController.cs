using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vebtech.CustomException;
using vebtech.Models;
using vebtech.Models.DTO;
using vebtech.Repositories.Abstract;
using vebtech.Utils;

namespace vebtech.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IValidateUtils _validateUtils;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository repository,
        ILogger<UsersController> logger,
        IValidateUtils validateUtils)
    {
        _repository = repository;
        _logger = logger;
        _validateUtils = validateUtils;
    }

    [ProducesResponseType(typeof(ICollection<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("getUsers")]
    public IActionResult Get(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SortParameters sortParameters,
        [FromQuery] FilterParameters filterParameters)
    {
        try
        {
            return Ok(new { users = _repository.Get(paginationParameters, sortParameters, filterParameters) });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"GetUsers method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }
    }

    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("user/{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            return Ok(new { user = await _repository.GetUser(id) });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"GetUser method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(UserDto userDto)
    {
        try
        {
            await _validateUtils.ValidateCreate(userDto);
            return Ok(new { user = await _repository.Create(userDto) });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"Create method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }
    }

    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPatch("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromForm] UserDto userDto)
    {
        try
        {
            await _validateUtils.ValidateUpdate(userDto);
            return Ok(new { user = await _repository.Update(id, userDto) });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"Update method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _repository.Delete(id);
            return Ok(new { message = "successfully" });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"Delete method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }

    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("role/{id:int}")]
    public async Task<IActionResult> CreateRole(int id, [FromForm] string roleName)
    {
        try
        {
            return Ok(new { role = await _repository.CreateRole(id, roleName) });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, $"CreateRole method error: {(int)ex.StatusCode}");
            return BadRequest(new { message = $"{ex.Value}. StatusCode: ${(int)ex.StatusCode}" });
        }
    }
}
