using Microsoft.EntityFrameworkCore;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Application.Services;
using TgBotGuide.Domain.Entities;
using TgBotGuide.Domain.Interfaces;
using TgBotGuide.Infrastructure;
using TgBotGuide.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<TgBotGuideDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IRepository<City>, Repository<City>>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<IRepository<Location>, Repository<Location>>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<IRepository<LocationCategory>, Repository<LocationCategory>>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddValidatorsFromAssemblyContaining<EmployeeRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();