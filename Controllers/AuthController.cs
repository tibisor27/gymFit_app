using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using GymFit.BE.Models;
using GymFit.BE.Data;
using GymFit.BE.DTOs;
using log4net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using GymFit.BE.Services;
using Microsoft.EntityFrameworkCore;

[Route("auth")]
[ApiController]
    public class AuthController : ODataController
    {
        private readonly GymFitDbContext _context;
        private readonly ILog _logger;
        private readonly JwtService _jwtService;
        private readonly IValidationService _validationService;

        public AuthController(
            GymFitDbContext context, 
            JwtService jwtService,
            IValidationService validationService)
        {
            _context = context;
            _jwtService = jwtService;
            _validationService = validationService;
            _logger = LogManager.GetLogger(typeof(AuthController));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Field = x.Key,
                        Errors = x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    });
                _logger.Error($"Login failed: Invalid model state. Errors: {errors}");
                return BadRequest(new { 
                    message = "Invalid login data", 
                    errors 
                });
            }

            _logger.Info($"Login attempt for email: {loginDto.Email}");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                _logger.Error($"Login failed: user not found for email {loginDto.Email}");
                return BadRequest(new { 
                    message = "Invalid credentials",
                    errors = new[] {
                        new {
                            Field = "Email",
                            Errors = new[] { "Invalid email or password" }
                        }
                    }
                });
            }

            var hashedPassword = HashPassword(loginDto.Password);
            if (user.Password != hashedPassword)
            {
                _logger.Error($"Login failed: wrong password for email {loginDto.Email}");
                return BadRequest(new { 
                    message = "Invalid credentials",
                    errors = new[] {
                        new {
                            Field = "Password",
                            Errors = new[] { "Invalid email or password" }
                        }
                    }
                });
            }

            var token = _jwtService.GenerateToken(user);
            _logger.Info($"Login successful for user {user.Email}");
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Field = x.Key,
                        Errors = x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    });
                _logger.Error($"Register failed: Invalid model state. Errors: {errors}");
                return BadRequest(new { message = "Invalid data", errors });
            }

            // Validate email format
            var (isEmailValid, emailErrors) = _validationService.ValidateEmail(registerDto.Email);
            if (!isEmailValid)
            {
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = new[] { new { Field = "Email", Errors = emailErrors } } 
                });
            }

            // Validate password strength
            var (isPasswordValid, passwordErrors) = _validationService.ValidatePassword(registerDto.Password);
            if (!isPasswordValid)
            {
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = new[] { new { Field = "Password", Errors = passwordErrors } } 
                });
            }

            // Validate phone number
            var (isPhoneValid, phoneErrors) = _validationService.ValidatePhoneNumber(registerDto.PhoneNumber);
            if (!isPhoneValid)
            {
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = new[] { new { Field = "PhoneNumber", Errors = phoneErrors } } 
                });
            }

            _logger.Info($"Register attempt for email: {registerDto.Email}");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                _logger.Error($"Register failed: email already exists {registerDto.Email}");
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = new[] { 
                        new { 
                            Field = "Email", 
                            Errors = new[] { "Email already exists!" } 
                        } 
                    } 
                });
            }

            // Validate age
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - registerDto.DateOfBirth.Year;
            if (registerDto.DateOfBirth > today.AddYears(-age)) age--;

            if (age < 16)
            {
                _logger.Error($"Register failed: user too young ({age} years old)");
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = new[] { 
                        new { 
                            Field = "DateOfBirth", 
                            Errors = new[] { "User must be at least 16 years old" } 
                        } 
                    } 
                });
            }

            if (age > 100)
            {
                _logger.Error($"Register failed: invalid age ({age} years old)");
                return BadRequest(new { 
                    message = "Validation failed", 
                    errors = new[] { 
                        new { 
                            Field = "DateOfBirth", 
                            Errors = new[] { "Invalid age" } 
                        } 
                    } 
                });
            }

            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = hashedPassword,
                UserRole = Role.Member,
                PhoneNumber = registerDto.PhoneNumber,
                DateOfBirth = registerDto.DateOfBirth
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.Info($"User registered successfully: {user.Email} (id: {user.Id})");
            return Ok(new { message = "User registered successfully!" });
        }

        private string HashPassword(string password)
        {
            // Implementarea hash-ului parolii
            using (SHA256 sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
