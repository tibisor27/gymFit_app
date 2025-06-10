using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using GymFit.API.Models;
using GymFit.API.Data;
using log4net;

[Route("odata/[controller]")]
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
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
    

}