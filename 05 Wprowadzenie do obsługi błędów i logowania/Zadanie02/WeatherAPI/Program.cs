using Serilog;
using WeatherAPI.Endpoints;
using WeatherAPI.Models;
using WeatherAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler("/api/error");

var cities = new List<CityDto>
{
    new CityDto { Id = 1, Name = "Warszawa" },
    new CityDto { Id = 2, Name = "Kraków" }
};

var idGenerator = new IdGenerator();
idGenerator.Next();
idGenerator.Next();

app.MapWeatherEndpoints();
app.MapCitiesEndpoints(cities, () => idGenerator.Next());

app.MapControllers();

app.Run();
