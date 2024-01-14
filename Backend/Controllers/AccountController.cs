using Backend.Models;
using Backend.Modules;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userManager;

        public AccountController(
            IUserService userManager
            )
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> RegisterAsync([FromBody] RegisterModel userModel, CancellationToken token)
        {
            var model = await _userManager.CreateUser(userModel, token);
            return Ok(model);

        }

        [HttpGet]
        public async Task<ActionResult<UserModel>> GetCurrentUserDataAync(CancellationToken token)
        {
            var model = await _userManager.GetCurrentLoggedInUserData(token);
            return Ok(model);

        }

        [HttpPost("update")]
        public async Task<ActionResult<UserModel>> UpdateUserData([FromBody] UserModel model, CancellationToken token)
        {
             await _userManager.UpdateUserData(model, token);
            return Ok();

        }


        [HttpPost("forget-password")]
        public async Task<ActionResult<string>> ForgetPassword([FromBody] ForgetPassword email)
        {
            await _userManager.ForgetPassword(email.Email);

            return Ok("Password reset initiated. Check your email for further instructions.");
        }

    }
}
