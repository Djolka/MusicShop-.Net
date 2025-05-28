using Microsoft.AspNetCore.Mvc;
using MusicShop.Models;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using System.Linq.Expressions;
using System.CodeDom;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MusicShop.Services;
using System.Text;


namespace MusicShop.Controllers
{
	[ApiController]
    [Authorize]
	[Route("users")]
	public class UsersController : ControllerBase
	{
		private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly JwtTokenService _jwtTokenService;
        PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

		public UsersController(AppDbContext context, IConfiguration config, JwtTokenService jwtTokenService)
		{
			_context = context;
            _config = config;
			_jwtTokenService = jwtTokenService;
        }

		[AllowAnonymous]
		[HttpGet("users")]
		public async Task<ActionResult<List<User>>> GetUsers()
		{
			var users = await _context.Users.ToListAsync();

			if (users == null)
			{
				return NotFound();
			}

			return Ok(users);
		}

		[HttpGet("user/{id}")]
		public async Task<ActionResult<User>> GetUserById(string id)
		{
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		[HttpGet("userByEmail/{email}")]
		public async Task<ActionResult<object>> GetUserByEmail(string email)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

			if (user == null)
			{
				return NotFound(new { found= false});
			}

			return Ok(new { found= true});
		}

        [AllowAnonymous]
        [HttpPost("login")]
		public async Task<ActionResult<User>> LoginUser([FromBody] User user)
		{
			var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(user.Email));

			if (userFound == null)
			{
				return NotFound(new { message= "User not found"});
			}

			var checkPassword = _passwordHasher.VerifyHashedPassword(user, userFound.Password, user.Password);
			if (checkPassword == PasswordVerificationResult.Success)
			{
                var token = _jwtTokenService.GenerateToken(userFound);
                return Ok(new { token, user = userFound });
			} else {
				return NotFound();
			}
		}

        [AllowAnonymous]
        [HttpPost("signup")]
		public async Task<ActionResult<User>> SignupUser([FromBody] User user)
		{
			var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(user.Email));

			if (userFound != null)
			{
				return BadRequest(new { message = "Signup failed" });
			}

			user.Password = _passwordHasher.HashPassword(user, user.Password);
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token, user = user });
		}

		[HttpPut("update/{id}")]
		public async Task<ActionResult<User>> UpdateUser([FromBody] UserUpdateDTO updatedUser, string id)
		{
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}
            if (!string.IsNullOrEmpty(updatedUser.PhoneNumber)) { 
				user.PhoneNumber = updatedUser.PhoneNumber;
			}

			if (!string.IsNullOrEmpty(updatedUser.Address)) { 
				user.Address = updatedUser.Address;
			}

            if (!string.IsNullOrEmpty(updatedUser.Country))
            {
                user.Country = updatedUser.Country;
            }

			try
			{
				await _context.SaveChangesAsync();
				return Ok(user);
			}
			catch (DbUpdateException ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[AllowAnonymous]
		[HttpDelete("users")]
		public async Task<ActionResult<List<User>>> DeleteAllUsers()
		{
			var usersToDelete = await _context.Users.ToListAsync();

			if (usersToDelete.Count == 0)
			{
				return NotFound("No users found to delete.");
			}

			_context.Users.RemoveRange(usersToDelete);
			await _context.SaveChangesAsync();

			return Ok(usersToDelete);
		}

		[AllowAnonymous]
		[HttpDelete("users/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound(new { message = "User not found" });
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return Ok(new { message = "User deleted successfully" });
		}
	}
}