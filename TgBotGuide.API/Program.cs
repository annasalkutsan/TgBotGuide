using Microsoft.EntityFrameworkCore;
using TgBotGuide.Application.Interfaces;
using TgBotGuide.Application.Mapping;
using TgBotGuide.Application.Services;
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

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<ILocationCategoryRepository, LocationCategoryRepository>();

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