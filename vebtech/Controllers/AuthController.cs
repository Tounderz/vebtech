using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using vebtech.Auth;
using vebtech.Repositories;
using vebtech.Models;
using vebtech.CustomException;
using System.Net;

namespace vebtech.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repository;

        public AuthController(IAuthRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("/sigin")]
        public IActionResult Signin(string email, string password)
        {
            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid email or password");
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }

        [HttpPost("/singup")]
        public IActionResult Signup(string email, string password)
        {
            if (_repository.IsExistEmail(email))
            {
                throw new HttpResponseException(HttpStatusCode.Conflict, "Admin with this email exist");
            }

            Admin admin = _repository.Signup(email, password);
            if (admin == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Invalid email or password");
            }

            return Signup(email, password);
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            Admin admin = _repository.Signin(email, password);
            if (admin == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, admin.Email),
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
