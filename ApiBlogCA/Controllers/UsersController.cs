using Domain.Constants;
using Domain.DomainServices;
using Domain.Models;
using Domain.Models.Dtos;
using Domain.Repository.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlogCA.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController() {
        }

        // GET: api/v1/Users    
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync([FromServices] UsersHandler handler) {
            return Ok(await handler.GetAllUsersAsync());
        }

        // GET: api/v1/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<User>> GetUser([FromServices] UsersHandler handler, int id) {
            var user = await handler.GetUserCompleteAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // DELETE: api/v1/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUser([FromServices] UsersHandler handler, int id) {
            await handler.DeleteUserAsync(id);
            return NoContent();
        }

        // PUT: api/v1/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> PutUser([FromServices] UsersHandler handler, int id, User user) {
            if (id != user.UserId)
                return BadRequest();
            await handler.UpdateUserAsync(id, user);
            return NoContent();
        }

        // PUT: api/v1/Users/blocked/5
        [HttpPut("blocked/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> PutUserBlocked([FromServices] UsersHandler handler, int id) {
            var user = await handler.UpdateUserBlockedAsync(id);
            if (user == null)
                return NotFound();
            return NoContent();
        }

        // PUT: api/v1/Users/Pending/5
        [HttpPut("Pending/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> PutUserPending([FromServices] UsersHandler handler, int id) {
            var user = await handler.UpdateUserPendingAsync(id);
            if (user == null)
                return NotFound();
            return NoContent();
        }

        // PUT: api/v1/Users/Active/5
        [HttpPut("Active/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> PutUserActive([FromServices] UsersHandler handler, int id) {
            var user = await handler.UpdateUserActiveAsync(id);
            if (user == null)
                return NotFound();
            return NoContent();
        }

        // GET: api/v1/Users/confirm/{id}/{token}
        [HttpGet("confirm/{id}/{token}")]
        [AllowAnonymous]
        public async Task<string> GetConfirmUser([FromServices] UsersHandler handler, int id, string token) {
            var response = await handler.ConfirmUserAsync(id, token);
            return response;
        }

        // POST: api/Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> PostUser([FromServices] UsersHandler handler, UserDto userDto) {
            var host = HttpContext.Request.Host.ToString(); 
            string response = await handler.RegisterUserAsync(userDto, host);
            if (response == string.Empty)
                return BadRequest();
            if (response == ErrorMessage.UserExists)       // verifica si usuario esta activo
                return NotFound(response);
            if (response == ErrorMessage.UserBlocked)       // verifica si usuario esta bloqueado
                return NotFound(response);

            return Created("GetUser", new { message = response });
        }

        // POST: api/ChangePassword
        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<ActionResult> PostChangePassword([FromServices] UsersHandler handler, ChangePassword changePassword) {
            if (changePassword == null)
                return BadRequest();
            var response = await handler.ChangeUserPasswordAsync(changePassword);
            if(response == ErrorMessage.EmailOrPassword)
                return NotFound(response);
            return Ok(new { message = response });
        }

        // POST: api/ResetPassword
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> PostResetPassword([FromServices] UsersHandler handler, ResetPassword resetPassword) {
            if (resetPassword == null)
                return BadRequest();
            var host = HttpContext.Request.Host.ToString();
            var response = await handler.ResetUserPasswordAsync(resetPassword, host);
            if (response == ErrorMessage.UserNotLogin)
                return NotFound(response);
            return Ok(new { message = response });
        }

        // POST: api/RegisterAdmin
        [HttpPost("registerAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult> PostUserAdmin([FromServices] UsersHandler handler, UserDto userDto) {
            if (userDto == null)
                return BadRequest();
            var response = await handler.RegisterAdminAsync(userDto);
            if (response == ErrorMessage.UserExists)
                return NotFound(response);
            return Created("GetUser", new { id = response });
        }

        // POST: api/Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> PostUserAsync([FromServices] UsersHandler handler, Login login) {
            if (login == null)
                return BadRequest();
            var response = await handler.LoginUserAsync(login);
            if (response == ErrorMessage.UserNotLogin || response == ErrorMessage.UserBlocked || response == ErrorMessage.ResetPassword || response == ErrorMessage.UserPending)
                return NotFound(response);
            return Ok(new { token = response });
        }

        ///////////////////////////// pruebas
        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Authenticated - {0}, {1}", User.Identity.Name, User.IsInRole(Roles.Admin));

        [HttpGet]
        [Route("testUser")]
        [Authorize(Roles = Roles.User)]
        public string TestUser() => "You are a User ";

        [HttpGet]
        [Route("testAdmin")]
        [Authorize(Roles = Roles.Admin)]
        public string TestAdmin() => "You are a Admin";
    }
}
