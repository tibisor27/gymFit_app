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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    
    // Înregistrează User pentru OData
    builder.EntitySet<User>("Users");
    
    return builder.GetEdmModel();
}
