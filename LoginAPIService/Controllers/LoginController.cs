using LoginAPIService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserRepository _userRepository;

        private readonly IConfiguration _configuration;

        public LoginController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpGet("Get")]
        [Authorize(Roles = "Admin")]
        public string Get()
        {
            return "Autorize users only";
        }

        [HttpPost]
        [Route("AddNewUser")]
        public IActionResult RegisterUser([FromBody] User userDetails)
        {
            if (userDetails == null || string.IsNullOrEmpty(userDetails.UserId))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please enter user details."
                });
            }

            var isUserAdded = _userRepository.RegisterUser(userDetails);

            return Ok(new ApiResponse
            {
                StatusCode = isUserAdded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable,
                Message = isUserAdded ? "Registered successfulyy, please login." : "Your email id is already regiseterd."
            });
        }

        [HttpPost]
        [Route("Admin")]
        public IActionResult ValidateUser([FromBody] User user)
        {
            if(user == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Username or password is incorrect"
                });
            }

            var loggedUser =_userRepository.ValidateUser(user);

            if(loggedUser != null)
            {
                var tokenString = GenerateJwtToken(loggedUser);

                loggedUser.Token = tokenString;

                return Ok(loggedUser);
            }
            else
            {
                return Ok(new ApiResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Please enter a valid credentials"
                });
            }
        }

        private string GenerateJwtToken(User userDetail)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("EmailId", userDetail.UserId),
                new Claim(ClaimTypes.Role, userDetail.IsAdminUser == true ? "Admin" : "User")
            };

            var jwtSecretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretKey")));
            var creds = new SigningCredentials(jwtSecretKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(60), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
