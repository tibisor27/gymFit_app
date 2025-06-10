using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using GymFit.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure OData
builder.Services.AddControllers().AddOData(options =>
{
    options.Select().Filter().OrderBy().Count().SetMaxTop(100);
    options.AddRouteComponents("odata", GetEdmModel());

});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<User>("Users");
    builder.EntitySet<GymClass>("GymClasses");
    return builder.GetEdmModel();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
