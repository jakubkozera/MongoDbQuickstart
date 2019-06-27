using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Models;
using Project.Core;
using Project.Identity;
using Project.Infrastructure.Mongo;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly UserManager _userManager;
        private readonly string[] _defaultUserRoles = { Role.User };

        public AccountController(IJwtHandler jwtHandler, UserManager userManager)
        {
            _jwtHandler = jwtHandler;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDto model)
        {
            var user = new User(model.Email, _defaultUserRoles);

            await _userManager.CreateAsync(user, model.Password);
            var token = await _jwtHandler.GenerateJwtToken(user.Email, user.Name, user.EntityId.ToString(), user.Roles);
            return Ok(token);

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserSignedIn>> Login([FromBody] LoginDto model)
        {
            if (!await _userManager.PasswordSignInAsync(model.Email, model.Password))
                throw new Exception("Invalid_credentials");

            var user = await _userManager.GetUserByNameAsync(model.Email);
            var token = await _jwtHandler.GenerateJwtToken(user.Email, user.Name, user.EntityId.ToString(), user.Roles);

            var result = new UserSignedIn()
            {
                Roles = user.Roles,
                AccessToken = token,
                Email = user.Name,
                Id = user.EntityId.ToString()
            };

            return Ok(result);
        }


        [Authorize]
        [HttpGet("Protected")]
        public async Task<object> Protected()
        {
            var user = this.User;
            return "Protected area";
        }


        [HttpGet("NotProtected")]
        public async Task<object> NotProtected()
        {

            return "Not Protected area";
        }

    }

}
