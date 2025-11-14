using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MusicShop.Models;
using MusicShop.Repositories;
using MusicShop.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicShop.Controllers
{
    [ApiController]
    [Authorize]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

        public UsersController(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        // GET /users/users
        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null)
                return NotFound();

            return Ok(users);
        }

        // GET /users/user/{id}
        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET /users/userByEmail/{email}
        [HttpGet("userByEmail/{email}")]
        public async Task<ActionResult<object>> GetUserByEmail(string email)
        {
            var exists = await _userRepository.GetUserByEmailAsync(email);
            return Ok(new { found = exists });
        }

        // POST /users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser([FromBody] UserLoginDTO user)
        {
            var userFound = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userFound == null)
                return NotFound(new { message = "User not found" });

            var checkPassword = _passwordHasher.VerifyHashedPassword(user, userFound.Password, user.Password);
            if (checkPassword != PasswordVerificationResult.Success)
                return Unauthorized(new { message = "Invalid password" });

            var token = _jwtTokenService.GenerateToken(userFound);

            return Ok(new { token, user = userFound });
        }

        // POST /users/signup
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult<User>> SignupUser([FromBody] UserSignupDTO signupUserDTO)
        {
            var exists = await _userRepository.GetUserByEmailAsync(signupUserDTO.Email);
            if (exists != null)
                return BadRequest(new { message = "Signup failed" });

            var user = new User
            {
                Name = signupUserDTO.Name,
                LastName = signupUserDTO.LastName,
                Email = signupUserDTO.Email,
                Password = _passwordHasher.HashPassword(signupUserDTO, signupUserDTO.Password),
                Address = string.Empty,
                Country = string.Empty,
                PhoneNumber = string.Empty,
                Role = "User" //default to User
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token, user });
        }

        // PUT /users/update/{id}
        [HttpPut("update/{id}")]
        public async Task<ActionResult<User>> UpdateUser([FromBody] UserUpdateDTO updatedUser, string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            if (updatedUser.PhoneNumber != null)
                user.PhoneNumber = updatedUser.PhoneNumber;

            if (updatedUser.Address != null)
                user.Address = updatedUser.Address;

            if (updatedUser.Country != null)
                user.Country = updatedUser.Country;

            //_userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Ok(user);
        }

        // DELETE /users/users
        [AllowAnonymous]
        [HttpDelete("users")]
        public async Task<ActionResult> DeleteAllUsers()
        {
            var deletedCount = await _userRepository.DeleteAllAsync();

            if (deletedCount == 0)
                return NotFound("No users found to delete.");

            return Ok($"Deleted {deletedCount} users.");
        }


        // DELETE /users/users/{id}
        [AllowAnonymous]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("setRole/{id}")]
        public async Task<IActionResult> SetRole(string id, [FromBody] string newRole)
        {
            if (newRole != "User" && newRole != "Admin") return BadRequest("Role not supported");

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Role = newRole;
            await _userRepository.SaveChangesAsync();

            return Ok($"User {user.Email} promoted to {newRole}");
        }
    }
}
