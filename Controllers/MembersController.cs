using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using GymFit.BE.Models;
using GymFit.BE.Data;
using GymFit.BE.DTOs;
using log4net;

public class MembersController : ODataController
{
    private readonly GymFitDbContext _context;
    private readonly ILog _logger;

    public MembersController(GymFitDbContext context)
    {
        _context = context;
        _logger = LogManager.GetLogger(typeof(MembersController));
    }

    // 🔍 GET - Toți membrii
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        try
        {
            _logger.Info("Getting all members");
            var members = await _context.Members
                .Include(m => m.User)  // ✅ Include User data!
                .ToListAsync();

            var memberDTOs = members.Select(member => new MemberDTO
            {
                Id = member.Id,
                
                // ✅ FLAT FIELDS pentru frontend
                MemberName = member.User.Name,
                MemberEmail = member.User.Email,
                MemberPhone = member.User.PhoneNumber,
                
                // 🏃 MEMBER DATA
                IsActive = member.IsActive,
                
                // 📋 NESTED pentru detalii complete (opțional)
                User = new UserDTO
                {
                    Id = member.User.Id,
                    Name = member.User.Name,
                    Email = member.User.Email,
                    PhoneNumber = member.User.PhoneNumber,
                    UserRole = member.User.UserRole,
                    DateOfBirth = member.User.DateOfBirth
                }
            }).ToList();

            return Ok(memberDTOs);
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting members", ex);
            return StatusCode(500, "Internal server error");
        }
    }

    // 🔍 GET - Un singur member
    [EnableQuery]
    public async Task<IActionResult> Get(int key)
    {
        try
        {
            _logger.Info($"Getting member with ID: {key}");
            var member = await _context.Members
                .Include(m => m.User)  // ✅ Include User data!
                .FirstOrDefaultAsync(m => m.Id == key);
            
            if (member == null)
            {
                return NotFound($"Member with ID {key} not found");
            }

            var memberDTO = new MemberDTO
            {
                Id = member.Id,
                
                // ✅ FLAT FIELDS pentru frontend
                MemberName = member.User.Name,
                MemberEmail = member.User.Email,
                MemberPhone = member.User.PhoneNumber,
                
                // 🏃 MEMBER DATA
                IsActive = member.IsActive,
                
                // 📋 NESTED pentru detalii complete
                User = new UserDTO
                {
                    Id = member.User.Id,
                    Name = member.User.Name,
                    Email = member.User.Email,
                    PhoneNumber = member.User.PhoneNumber,
                    UserRole = member.User.UserRole,
                    DateOfBirth = member.User.DateOfBirth
                }
            };

            return Ok(memberDTO);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error getting member {key}", ex);
            return StatusCode(500, "Internal server error");
        }
    }

    // 🏃 POST - Creez member details pentru un user existent
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateMemberDTO createDto)
    {
        try
        {
            _logger.Info("🏃 POST member request started");
            
            if (createDto == null)
            {
                _logger.Error("❌ CreateMemberDTO is null");
                return BadRequest("Member data is required");
            }
            
            _logger.Info($"🏃 POST member for user {createDto.UserId}");

            // 🛡️ Validez modelul
            if (!ModelState.IsValid)
            {
                _logger.Error("Invalid model state for member creation");
                return BadRequest(ModelState);
            }

            // 🔍 Verific dacă user-ul există
            var user = await _context.Users.FindAsync(createDto.UserId);
            if (user == null)
            {
                return NotFound($"User cu ID {createDto.UserId} nu există");
            }

            // 🛡️ Verific dacă user-ul nu e deja member
            var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.UserId == createDto.UserId);
            if (existingMember != null)
            {
                return BadRequest($"User-ul cu ID {createDto.UserId} este deja member");
            }

            // 🏃 Creez member-ul
            var member = new Member
            {
                UserId = createDto.UserId,
                IsActive = createDto.IsActive
            };

            _context.Members.Add(member);

            // 🔄 Actualizez rolul user-ului la Member
            user.UserRole = Role.Member;

            await _context.SaveChangesAsync();

            // 🎯 Returnez răspunsul
            var response = new MemberResponseDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                IsActive = member.IsActive,
                UserName = user.Name,
                UserEmail = user.Email
            };

            _logger.Info($"✅ Member created successfully: ID {member.Id} for user {user.Id}");
            return Created($"/odata/Members({member.Id})", response);
        }
        catch (Exception ex)
        {
            _logger.Error($"❌ Error creating member: {ex.Message} | Inner: {ex.InnerException?.Message}");
            Console.WriteLine(ex.ToString());
            return StatusCode(500, "Internal server error");
        }
    }

    // ✅ PATCH - Actualizare PARȚIALĂ member!
    [HttpPatch]
    public async Task<IActionResult> Patch(int key, [FromBody] UpdateMemberDTO updateDto)
    {
        try
        {
            _logger.Info($"🔄 PATCH member {key}");

            // 🛡️ Validez modelul cu Data Annotations
            if (!ModelState.IsValid)
            {
                _logger.Error($"Invalid model state for member {key}");
                return BadRequest(ModelState);
            }

            // 🔍 Găsesc member-ul existent
            var member = await _context.Members
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == key);

            if (member == null)
            {
                return NotFound($"Member cu ID {key} nu există");
            }

            // ✅ ACTUALIZEZ DOAR câmpurile care NU sunt NULL!
            if (updateDto.IsActive.HasValue)
            {
                member.IsActive = updateDto.IsActive.Value;
                _logger.Info($"Updated IsActive to {updateDto.IsActive.Value} for member {key}");
            }

            // Salvez modificările
            await _context.SaveChangesAsync();

            // Returnez DTO-ul actualizat
            var response = new MemberResponseDTO
            {
                Id = member.Id,
                UserId = member.UserId,
                IsActive = member.IsActive,
                UserName = member.User.Name,
                UserEmail = member.User.Email
            };

            _logger.Info($"✅ Member {key} updated successfully");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error($"❌ Error updating member {key}: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    // ❌ DELETE - Șterge member (și schimbă user role la Admin)
    [HttpDelete]
    public async Task<IActionResult> Delete(int key)
    {
        try
        {
            _logger.Info($"🗑️ DELETE member {key}");

            // 🔍 Găsesc member-ul cu user-ul asociat
            var member = await _context.Members
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == key);

            if (member == null)
            {
                return NotFound($"Member cu ID {key} nu există");
            }

            // 🔄 Schimb user role-ul înapoi la Admin (sau poți să-l lași fără rol specific)
            member.User.UserRole = Role.Admin;

            // 🗑️ Șterg member-ul
            _context.Members.Remove(member);

            await _context.SaveChangesAsync();

            _logger.Info($"✅ Member {key} deleted successfully, user {member.UserId} role changed to Admin");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.Error($"❌ Error deleting member {key}: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
} 