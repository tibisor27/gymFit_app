using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using GymFit.BE.Models;
using GymFit.BE.Data;
using GymFit.BE.DTOs;
using log4net;


  public class TrainersController : ODataController
    {
        private readonly GymFitDbContext _context;
        private readonly ILog _logger;

        public TrainersController(GymFitDbContext context)
        {
            _context = context;
            _logger = LogManager.GetLogger(typeof(TrainersController));
        }
        
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.Info("Getting all trainers");
                var trainers = await _context.Trainers
                    .Include(t => t.User)  // ✅ Include User data!
                    .ToListAsync();

                var trainerDTOs = trainers.Select(trainer => new TrainerDTO
                {
                    Id = trainer.Id,
                    
                    // ✅ FLAT FIELDS pentru frontend
                    TrainerName = trainer.User.Name,
                    TrainerEmail = trainer.User.Email,
                    TrainerPhone = trainer.User.PhoneNumber,
                    
                    // 🏋️ TRAINER DATA
                    Experience = trainer.Experience,
                    Introduction = trainer.Introduction,
                    
                    // 📋 NESTED pentru detalii complete (opțional)
                    User = new UserDTO
                    {
                        Id = trainer.User.Id,
                        Name = trainer.User.Name,
                        Email = trainer.User.Email,
                        PhoneNumber = trainer.User.PhoneNumber,
                        UserRole = trainer.User.UserRole,
                        DateOfBirth = trainer.User.DateOfBirth
                    }
                }).ToList();

                return Ok(trainerDTOs);
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting trainers", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Get(int key)
        {
            try
            {
                _logger.Info($"Getting trainer with ID: {key}");
                var trainer = await _context.Trainers
                    .Include(t => t.User)  // ✅ Include User data!
                    .FirstOrDefaultAsync(t => t.Id == key);
                
                if (trainer == null)
                {
                    return NotFound($"Trainer with ID {key} not found");
                }

                var trainerDTO = new TrainerDTO
                {
                    Id = trainer.Id,
                    
                    // ✅ FLAT FIELDS pentru frontend
                    TrainerName = trainer.User.Name,
                    TrainerEmail = trainer.User.Email,
                    TrainerPhone = trainer.User.PhoneNumber,
                    
                    // 🏋️ TRAINER DATA
                    Experience = trainer.Experience,
                    Introduction = trainer.Introduction,
                    
                    // 📋 NESTED pentru detalii complete
                    User = new UserDTO
                    {
                        Id = trainer.User.Id,
                        Name = trainer.User.Name,
                        Email = trainer.User.Email,
                        PhoneNumber = trainer.User.PhoneNumber,
                        UserRole = trainer.User.UserRole,
                        DateOfBirth = trainer.User.DateOfBirth
                    }
                };

                return Ok(trainerDTO);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting trainer {key}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // 🏋️ POST - Creez trainer details pentru un user existent
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTrainerDTO createDto)
        {
            try
            {
                _logger.Info($"🏋️ POST trainer for user {createDto.UserId}");

                // 🛡️ Validez modelul
                if (!ModelState.IsValid)
                {
                    _logger.Error("Invalid model state for trainer creation");
                    return BadRequest(ModelState);
                }

                // 🔍 Verific dacă user-ul există
                var user = await _context.Users.FindAsync(createDto.UserId);
                if (user == null)
                {
                    return NotFound($"User cu ID {createDto.UserId} nu există");
                }

                // 🛡️ Verific dacă user-ul nu e deja trainer
                var existingTrainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == createDto.UserId);
                if (existingTrainer != null)
                {
                    return BadRequest($"User-ul cu ID {createDto.UserId} este deja trainer");
                }

                // 🏋️ Creez trainer-ul
                var trainer = new Trainer
                {
                    UserId = createDto.UserId,
                    Experience = createDto.Experience,
                    Introduction = createDto.Introduction
                };

                _context.Trainers.Add(trainer);

                // 🔄 Actualizez rolul user-ului la Trainer
                user.UserRole = Role.Trainer;

                await _context.SaveChangesAsync();

                // 🎯 Returnez răspunsul
                var response = new TrainerResponseDTO
                {
                    Id = trainer.Id,
                    UserId = trainer.UserId,
                    Experience = trainer.Experience,
                    Introduction = trainer.Introduction,
                    UserName = user.Name,
                    UserEmail = user.Email
                };

                _logger.Info($"✅ Trainer created successfully: ID {trainer.Id} for user {user.Id}");
                return Created($"/odata/Trainers({trainer.Id})", response);
            }
            catch (Exception ex)
            {
                _logger.Error($"❌ Error creating trainer: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // ✅ PATCH - Actualizare PARȚIALĂ trainer!
        [HttpPatch]
        public async Task<IActionResult> Patch(int key, [FromBody] UpdateTrainerDTO updateDto)
        {
            try
            {
                _logger.Info($"🔄 PATCH trainer {key}");

                // 🛡️ Validez modelul cu Data Annotations
                if (!ModelState.IsValid)
                {
                    _logger.Error($"Invalid model state for trainer {key}");
                    return BadRequest(ModelState);
                }

                // 🔍 Găsesc trainer-ul existent
                var trainer = await _context.Trainers
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == key);

                if (trainer == null)
                {
                    return NotFound($"Trainer cu ID {key} nu există");
                }

                // ✅ ACTUALIZEZ DOAR câmpurile care NU sunt NULL!
                if (updateDto.Experience != null)
                {
                    trainer.Experience = updateDto.Experience;
                    _logger.Info($"Updated experience for trainer {key}");
                }

                if (updateDto.Introduction != null)
                {
                    trainer.Introduction = updateDto.Introduction;
                    _logger.Info($"Updated introduction for trainer {key}");
                }

                // Salvez modificările
                await _context.SaveChangesAsync();

                // Returnez DTO-ul actualizat
                var response = new TrainerResponseDTO
                {
                    Id = trainer.Id,
                    UserId = trainer.UserId,
                    Experience = trainer.Experience,
                    Introduction = trainer.Introduction,
                    UserName = trainer.User.Name,
                    UserEmail = trainer.User.Email
                };

                _logger.Info($"✅ Trainer {key} updated successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"❌ Error updating trainer {key}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // ❌ DELETE - Șterge trainer (și schimbă user role la Member)
        [HttpDelete]
        public async Task<IActionResult> Delete(int key)
        {
            try
            {
                _logger.Info($"🗑️ DELETE trainer {key}");

                // 🔍 Găsesc trainer-ul cu user-ul asociat
                var trainer = await _context.Trainers
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == key);

                if (trainer == null)
                {
                    return NotFound($"Trainer cu ID {key} nu există");
                }

                // 🔄 Schimb user role-ul înapoi la Member
                trainer.User.UserRole = Role.Member;

                // 🗑️ Șterg trainer-ul
                _context.Trainers.Remove(trainer);

                await _context.SaveChangesAsync();

                _logger.Info($"✅ Trainer {key} deleted successfully, user {trainer.UserId} role changed to Member");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error($"❌ Error deleting trainer {key}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }