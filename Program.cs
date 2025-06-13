using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using GymFit.BE.Data;
using GymFit.BE.Models;
using GymFit.BE.DTOs; // ADĂUGĂM DTOs!
using System.Text.Json.Serialization;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GymFit.BE.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔧 CONFIGURE LOGGING
    var logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
    if (!Directory.Exists(logsPath))
    {
        Directory.CreateDirectory(logsPath);
    }

    log4net.Util.LogLog.InternalDebugging = true; // Opțional: pentru debugging log4net
    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config")); // Configurează log4net din fișierul "log4net.config" (XML)
    var logger = LogManager.GetLogger(typeof(Program));

// 🔧 CONFIGURE JSON SERIALIZATION AND ODATA
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization options
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keep original property names
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
        options.EnableQueryFeatures(); // Enable all query features
        options.RouteOptions.EnableUnqualifiedOperationCall = true;
        options.RouteOptions.EnableQualifiedOperationCall = true;
        options.RouteOptions.EnableKeyInParenthesis = true;
        options.RouteOptions.EnableKeyAsSegment = true;
        options.AddRouteComponents("odata", GetEdmModel());
    });

// 🔒 CONFIGURE JWT AUTHENTICATION
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new Exception("JWT Key missing!");
var jwtIssuer = jwtSettings["Issuer"] ?? throw new Exception("JWT Issuer missing!");
var jwtAudience = jwtSettings["Audience"] ?? throw new Exception("JWT Audience missing!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 🔒 CONFIGURE DB CONTEXT
builder.Services.AddDbContext<GymFitDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("gymFitConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IValidationService, ValidationService>();

var app = builder.Build();

app.UseCors();
app.UseRouting(); // Add explicit routing
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    
    // Înregistrează DOAR modelele esențiale pentru OData - SIMPLU!
    builder.EntitySet<User>("Users");
    builder.EntitySet<Member>("Members");
    builder.EntitySet<Trainer>("Trainers");
    
    // DTOs pentru OData
    builder.EntitySet<UserDTO>("UserDTOs");
    builder.EntitySet<MemberDTO>("MemberDTOs");
    builder.EntitySet<TrainerDTO>("TrainerDTOs");
    
    return builder.GetEdmModel();
}
