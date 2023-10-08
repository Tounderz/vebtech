using vebtech.Models.DTO;

namespace vebtech.Utils;

public interface IValidateUtils
{
    Task ValidateCreate(UserDto userDto);
    Task ValidateUpdate(UserDto userDto);
}
