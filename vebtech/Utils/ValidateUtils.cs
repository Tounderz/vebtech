using System.Net;
using vebtech.Application.Services.Interfaces;
using vebtech.CustomException;
using vebtech.Domain.Models.DTO;
using vebtech.Utils.Interfaces;

namespace vebtech.Utils;

public class ValidateUtils : IValidateUtils
{
    private readonly IUserService _userService;

    public ValidateUtils(IUserService userService)
    {
        _userService = userService;
    }

    public async Task ValidateCreate(UserDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Email) || userDto.Age == null || string.IsNullOrEmpty(userDto.Name))
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "All fields required");
        }

        await ValidateEmail(userDto.Email);
        ValidateAge(userDto.Age);
    }

    public async Task ValidateUpdate(UserDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Email) && userDto.Age == null && string.IsNullOrEmpty(userDto.Name))
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "At least one field required");
        }

        if (userDto.Age != null)
        {
            ValidateAge(userDto.Age);
        }

        if (!string.IsNullOrEmpty(userDto.Email))
        {
            await ValidateEmail(userDto.Email);
        }
    }

    public void ValidateCreateRole(RoleDto roleDto)
    {
        if (string.IsNullOrEmpty(roleDto.Name) || !string.IsNullOrEmpty(roleDto.UserId))
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "All fields required");
        }

        if (!int.TryParse(roleDto.UserId, out int result) || result <= 0)
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "UserId must be a valid positive number");
        }
    }

    public void ValidateAdmin(AdminDto adminDto)
    {
        if (adminDto == null || string.IsNullOrEmpty(adminDto.Email) || string.IsNullOrEmpty(adminDto.Password))
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "All fields required");
        }
    }

    private async Task ValidateEmail(string? email)
    {
        if (!string.IsNullOrEmpty(email) && await _userService.IsEmailExist(email))
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "User with this email is exist");
        }
    }

    private void ValidateAge(string age)
    {
        if (!int.TryParse(age, out int result) || result <= 0)
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Age must be a valid positive number");
        }
    }
}