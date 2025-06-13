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

        public AuthController(GymFitDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = LogManager.GetLogger(typeof(AuthController));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto) //loginDto contine email si parola care vine de la frontend
        {   //user va returna primul obiect (user) din baza de date care are emailul la fel cu cel din login
            _logger.Info($"Login attempt for email: {loginDto.Email}");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email); //verificam daca userul exista in baza de date
            if (user == null)
            {
                _logger.Error($"Login failed: user not found for email {loginDto.Email}");
                return Unauthorized("Email sau parolă incorectă!");
            }

            // Verifică parola (hash, etc)
            var hashedPassword = HashPassword(loginDto.Password);
            if (user.Password != hashedPassword)
            {
                _logger.Error($"Login failed: wrong password for email {loginDto.Email}");
                return Unauthorized("Email sau parolă incorectă!");
            }

            var token = _jwtService.GenerateToken(user);
            _logger.Info($"Login successful for user {user.Email}");
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO registerDto)
        {
            _logger.Info($"Register attempt for email: {registerDto.Email}");
            // 1. Verifică dacă emailul există deja
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                _logger.Error($"Register failed: email already exists {registerDto.Email}");
                return BadRequest("Email already exists!");
            }

            // 2. Hash-uiește parola
            var hashedPassword = HashPassword(registerDto.Password);

            // 3. Creează userul
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = hashedPassword,
                UserRole = registerDto.UserRole, // sau default: Role.Member
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
