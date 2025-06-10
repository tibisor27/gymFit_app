using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using GymFit.API.Models;

namespace GymFit.API.Controllers
{
    public class UsersController : ODataController
    {
        // Temporary in-memory data for testing
        private static readonly List<User> Users = new()
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", UserType = "Member", CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", UserType = "Trainer", CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new User { Id = 3, FirstName = "Mike", LastName = "Johnson", Email = "mike.johnson@email.com", UserType = "Member", CreatedAt = DateTime.UtcNow.AddDays(-7) },
            new User { Id = 4, FirstName = "Sarah", LastName = "Wilson", Email = "sarah.wilson@email.com", UserType = "Admin", CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new User { Id = 5, FirstName = "David", LastName = "Brown", Email = "david.brown@email.com", UserType = "Trainer", CreatedAt = DateTime.UtcNow.AddDays(-1) }
        };

        [EnableQuery]
        [HttpGet]
        public IQueryable<User> Get()
        {
            return Users.AsQueryable();
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get(int key)
        {
            var user = Users.FirstOrDefault(u => u.Id == key);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Id = Users.Max(u => u.Id) + 1;
            user.CreatedAt = DateTime.UtcNow;
            Users.Add(user);

            return Created(user);
        }

        [HttpPut]
        public IActionResult Put(int key, [FromBody] User user)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == key);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.UserType = user.UserType;
            existingUser.IsActive = user.IsActive;

            return Updated(existingUser);
        }

        [HttpDelete]
        public IActionResult Delete(int key)
        {
            var user = Users.FirstOrDefault(u => u.Id == key);
            if (user == null)
            {
                return NotFound();
            }

            Users.Remove(user);
            return NoContent();
        }
    }
} 