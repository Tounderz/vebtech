using System.Net;
using vebtech.CustomException;
using vebtech.DTO;
using vebtech.Models;
using vebtech.Repositories;
using vebtech.Repositories.Impl;

namespace vebtech.Utils
{
    public class ValidateUtils
    {
        private static bool IsAnyFieldsNull (UserDTO user)
        {
            return user.Email == null || user.Age == null || user.Name == null;
        }

        private static bool IsAgePositive (int age)
        {
            return age > 0;
        }

        private static void ValidateEmail(IUserRepository _repository, string? email)
        {
            if (!string.IsNullOrEmpty(email) && _repository.IsEmailExist(email))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "User with this email is exist");
            }
        }

        private static void ValidateAge(int? age)
        {
            if (age != null && !IsAgePositive((int)age))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Age is must be positive");
            }
        }

        public static void ValidateCondition(IUserRepository repository, UserDTO user)
        {
            ValidateAge(user.Age);
            ValidateEmail(repository, user.Email);
        }

        public static void ValidateCreate(IUserRepository repository, UserDTO user)
        {
            IsAnyFieldsNull(user);
            ValidateCondition(repository, user);
        }
    }
}
