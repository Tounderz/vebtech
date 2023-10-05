using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;
using vebtech.CustomException;
using vebtech.DTO;
using vebtech.Models;
using vebtech.Repositories;
using vebtech.Repositories.Impl;
using vebtech.Utils;

namespace vebtech.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository repository, ILogger<UserController> logger) 
        { 
            _repository = repository;
            _logger = logger;
        }

        [HttpGet(Name = "Get Users")]
        public IActionResult Get([FromQuery] PaginationParameters paginationParameters, [FromQuery] SortParameters sortParameters)
        {
            if (!string.IsNullOrEmpty(sortParameters.OrderBy))
            {
                Type type = typeof(User);
                var field = type.GetProperty(sortParameters.OrderBy[0].ToString().ToUpper() + sortParameters.OrderBy.Substring(1).ToLower());

                if (field == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Not valid sort field");
                }
            }

            return Ok(_repository.Get(paginationParameters, sortParameters));
        }

        [HttpPost(Name = "Create User")]
        public IActionResult Create(UserDTO userDTO) 
        {
            ValidateUtils.ValidateCreate(_repository, userDTO);
            return Ok( new { user = _repository.Create(userDTO) });
        }

        [HttpGet("{id}", Name = "Get User By Id")]
        public User GetUser(int id) 
        {
            return _repository.GetUser(id); 
        }

        [HttpDelete("{id}", Name = "Delete User")]
        public void Delete(int id) 
        {
            _repository.Delete(id);
        }

        [HttpPut("{id}", Name = "Update User")]
        public IActionResult Update(int id,  [FromBody] UserDTO user) 
        {
            ValidateUtils.ValidateCondition(_repository, user);
            return Ok(new { user = _repository.Update(id, user) });
        }

        [HttpPost("{id}/role", Name = "Create Role")]
        public IActionResult CreateRole(int id, [FromBody] string roleName)
        {
            return Ok(new { role = _repository.CreateRole(id, roleName) });
        }
    }
}
