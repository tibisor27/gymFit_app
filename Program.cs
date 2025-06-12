using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using GymFit.BE.Data;
using GymFit.BE.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
        options.AddRouteComponents("odata", GetEdmModel());
    });

builder.Services.AddDbContext<GymFitDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("gymFitConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    
    // Înregistrează toate entitățile pentru OData
    builder.EntitySet<User>("Users");
    builder.EntitySet<Members>("Members");
    builder.EntitySet<Trainers>("Trainers");
    builder.EntitySet<Session>("Sessions");
    builder.EntitySet<Membership>("Memberships");
    builder.EntitySet<MembershipType>("MembershipTypes");
    builder.EntitySet<GymClass>("GymClasses");
    
    return builder.GetEdmModel();
}
