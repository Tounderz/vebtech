using System.Net;
using vebtech.CustomException;
using vebtech.Models.DTO;
using vebtech.Repositories.Abstract;

namespace vebtech.Utils;

public class ValidateUtils : IValidateUtils
{
    private readonly IUserRepository _userRepository;

    public ValidateUtils(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ValidateCreate(UserDto userDto)
    {
        if (userDto.Email == null || userDto.Age == null || userDto.Name == null)
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "All fields required");
        }

        ValidateEmail(userDto.Email);
        ValidateAge(userDto.Age);
    }

    public async Task ValidateUpdate(UserDto userDto)
    {
        if (userDto.Email == null && userDto.Age == null && userDto.Name == null)
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

    private async Task ValidateEmail(string? email)
    {
        if (!string.IsNullOrEmpty(email) && await _userRepository.IsEmailExist(email))
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "User with this email is exist");
        }
    }

    private void ValidateAge(int? age)
    {
        if (age <= 0)
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "Age is must be positive");
        }
    }
}