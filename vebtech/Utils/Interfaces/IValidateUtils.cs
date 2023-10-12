using vebtech.Domain.Models.DTO;

namespace vebtech.Utils.Interfaces;

public interface IValidateUtils
{
    Task ValidateCreate(UserDto userDto);
    Task ValidateUpdate(UserDto userDto);
    void ValidateAdmin(AdminDto adminDto);
    void ValidateCreateRole(RoleDto roleDto);
}
