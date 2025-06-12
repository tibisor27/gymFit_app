using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using GymFit.BE.Models;
using GymFit.BE.Data;
using log4net;

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

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.Info("Getting all users");
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting users", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _logger.Info($"Getting user with ID: {id}");
                var user = await _context.Users.FindAsync(id);
                
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting user {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.Info($"Creating new user: {user.Email}");
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                return Created($"/odata/Users/{user.Id}", user);
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating user", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}