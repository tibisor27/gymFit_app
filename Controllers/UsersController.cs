using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using GymFit.BE.Models;
using GymFit.BE.Data;
using GymFit.BE.DTOs;
using log4net;
using System.Security.Cryptography;
using System.Text;

namespace GymFit.BE.Controllers
{
    public class UsersController : ODataController
    {
        private readonly GymFitDbContext _context;
        private readonly ILog _logger;

        public UsersController(GymFitDbContext context)
        {
            _context = context;
            _logger = LogManager.GetLogger(typeof(UsersController));
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.Info("Getting all users");
                var users = await _context.Users.ToListAsync();
                
                var userDTOs = users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserRole = u.UserRole,
                    DateOfBirth = u.DateOfBirth
                }).ToList();
                
                return Ok(userDTOs);
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting users", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Get(int key)
        {
            try
            {
                _logger.Info($"Getting user with ID: {key}");
                var user = await _context.Users.FindAsync(key);
                
                if (user == null)
                {
                    return NotFound($"User with ID {key} not found");
                }
                
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = user.UserRole,
                    DateOfBirth = user.DateOfBirth
                };
                
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting user {key}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Post([FromBody] CreateUserDTO createUserDTO)
        {
            try
            {
                _logger.Info("=== POST REQUEST STARTED ===");
                _logger.Info($"Creating user: {createUserDTO.Email}");
                
                if (createUserDTO == null)
                {
                    return BadRequest("User data is required");
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == createUserDTO.Email);
                if (existingUser != null)
                {
                    _logger.Error($"❌ Email already exists: {createUserDTO.Email}");
                    return BadRequest($"Email {createUserDTO.Email} already exists");
                }

                string hashedPassword = HashPassword(createUserDTO.Password);

                var user = new User
                {
                    Name = createUserDTO.Name,
                    Email = createUserDTO.Email,
                    Password = hashedPassword,
                    UserRole = createUserDTO.UserRole,
                    PhoneNumber = createUserDTO.PhoneNumber,
                    DateOfBirth = createUserDTO.DateOfBirth
                };

                _logger.Info($"✅ Validation passed - Creating user: {user.Email}");
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                _logger.Info($"✅ User created successfully with ID: {user.Id}");
                
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = user.UserRole,
                    DateOfBirth = user.DateOfBirth
                };
                
                return Ok(userDTO); // SIMPLU! Fără OData Created
            }
            catch (Exception ex)
            {
                _logger.Error($"❌ EXCEPTION in POST: {ex.Message}");
                _logger.Error($"Stack trace: {ex.StackTrace}");
                
                return StatusCode(500, new { 
                    message = "Internal server error", 
                    error = ex.Message 
                });
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<IActionResult> Put(int key, [FromBody] UserDTO userDTO)
        {
            try
            {
                if (key != userDTO.Id)
                {
                    return BadRequest("ID mismatch");
                }

                var existingUser = await _context.Users.FindAsync(key);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {key} not found");
                }

                existingUser.Name = userDTO.Name;
                existingUser.Email = userDTO.Email;
                existingUser.UserRole = userDTO.UserRole;
                existingUser.PhoneNumber = userDTO.PhoneNumber;
                existingUser.DateOfBirth = userDTO.DateOfBirth;

                await _context.SaveChangesAsync();
                
                return Updated(userDTO);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error updating user {key}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // ✅ PATCH = Actualizare PARȚIALĂ!
        [HttpPatch]
        public async Task<IActionResult> Patch(int key, [FromBody] UpdateUserDTO updateDto)
        {
            try
            {
                _logger.Info($"PATCH request for user {key}");
                
                if (!ModelState.IsValid)
                {
                    _logger.Error($"Invalid model state for user {key}");
                    return BadRequest(ModelState);
                }

                if (updateDto.DateOfBirth.HasValue && updateDto.DateOfBirth.Value > DateOnly.FromDateTime(DateTime.Now.AddYears(-16)))
                {
                    return BadRequest("Utilizatorul trebuie să aibă minim 16 ani");
                }

                if (updateDto.Email != null)
                {
                    // Verific dacă email-ul există deja la alt user
                    var emailExists = await _context.Users.AnyAsync(u => u.Email == updateDto.Email && u.Id != key);
                    if (emailExists)
                    {
                        return BadRequest("Email-ul este deja folosit");
                    }
                }
                
                // Găsesc user-ul existent
                var existingUser = await _context.Users.FindAsync(key);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {key} not found");
                }

                // ✅ ACTUALIZEZ DOAR câmpurile care NU sunt NULL!
                if (updateDto.Name != null)
                {
                    existingUser.Name = updateDto.Name;
                    _logger.Info($"Updated name to: {updateDto.Name}");
                }
                
                if (updateDto.Email != null)
                {
                    existingUser.Email = updateDto.Email;
                    _logger.Info($"Updated email to: {updateDto.Email}");
                }
                
                if (updateDto.UserRole.HasValue)  // Pentru enum nullable
                {
                    existingUser.UserRole = updateDto.UserRole.Value;
                    _logger.Info($"Updated role to: {updateDto.UserRole}");
                }
                
                if (updateDto.PhoneNumber != null)
                {
                    existingUser.PhoneNumber = updateDto.PhoneNumber;
                    _logger.Info($"Updated phone to: {updateDto.PhoneNumber}");
                }
                
                if (updateDto.DateOfBirth.HasValue)  // Pentru DateOnly nullable
                {
                    existingUser.DateOfBirth = updateDto.DateOfBirth.Value;
                    _logger.Info($"Updated birth date to: {updateDto.DateOfBirth}");
                }

                // Salvez modificările
                await _context.SaveChangesAsync();
                
                // Returnez DTO-ul actualizat
                var userDTO = new UserDTO
                {
                    Id = existingUser.Id,
                    Name = existingUser.Name,
                    Email = existingUser.Email,
                    PhoneNumber = existingUser.PhoneNumber,
                    UserRole = existingUser.UserRole,
                    DateOfBirth = existingUser.DateOfBirth
                };
                
                _logger.Info($"✅ User {key} updated successfully");
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.Error($"❌ Error in PATCH user {key}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Delete(int key)
        {
            try
            {
                var user = await _context.Users.FindAsync(key);
                if (user == null)
                {
                    return NotFound($"User with ID {key} not found");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting user {key}", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}