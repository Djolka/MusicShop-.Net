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
using System;

namespace MusicShop.Controllers
{
	[ApiController]
	[Route("users")]
	public class UsersController : ControllerBase
	{
		private readonly AppDbContext _context;
		PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

		public UsersController(AppDbContext context)
		{
			_context = context;
		}

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
				return Ok(userFound);
			} else {
				return NotFound();
			}
		}

		[HttpPost("signup")]
		public async Task<ActionResult<User>> SignupUser([FromBody] User user)
		{
			var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(user.Email));

			if (userFound != null)
			{
				return BadRequest();
			}

			user.Password = _passwordHasher.HashPassword(user, user.Password);
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return Ok(user);
		}

		[HttpPut("update/{id}")]
		public async Task<ActionResult<User>> UpdateUser([FromBody] User updatedUser, string id)
		{
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			user.Name = updatedUser.Name;
			user.LastName = updatedUser.LastName;
			user.Email = updatedUser.Email;
			user.Password = user.Password = _passwordHasher.HashPassword(user, updatedUser.Password);
			user.Address = updatedUser.Address;
			user.PhoneNumber = updatedUser.PhoneNumber;
			user.Country = updatedUser.Country;

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