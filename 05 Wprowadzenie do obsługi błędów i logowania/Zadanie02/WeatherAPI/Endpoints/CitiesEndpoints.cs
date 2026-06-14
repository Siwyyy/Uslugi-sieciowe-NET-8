using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;

namespace WeatherAPI.Endpoints;

public static class CitiesEndpoints
{
    public static IEndpointRouteBuilder MapCitiesEndpoints(
        this IEndpointRouteBuilder app,
        List<CityDto> cities,
        Func<int> nextId)
    {
        app.MapPost("/cities", ([FromBody] AddCityRequest request) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest(new { message = "Nazwa miasta nie może być pusta." });
            }

            var normalized = request.Name.Trim();

            if (cities.Any(c => c.Name.Equals(normalized, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Conflict(new { message = $"Miasto '{normalized}' już istnieje." });
            }

            var newCity = new CityDto { Id = nextId(), Name = normalized };
            cities.Add(newCity);
            return Results.Created($"/cities/{newCity.Id}", newCity);
        })
        .WithName("AddCity")
        .WithOpenApi();

        app.MapGet("/cities", () => Results.Ok(cities))
            .WithName("GetCities")
            .WithOpenApi();

        app.MapGet("/cities/{id}", (int id) =>
        {
            var city = cities.FirstOrDefault(c => c.Id == id);
            return city == null
                ? Results.NotFound(new { message = $"Miasto o ID {id} nie znalezione." })
                : Results.Ok(city);
        })
        .WithName("GetCityById")
        .WithOpenApi();

        app.MapPut("/cities/{id:int}", (int id, [FromBody] UpdateCityRequest request) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest(new { message = "Nazwa miasta nie może być pusta." });
            }

            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return Results.NotFound(new { message = $"Miasto o ID {id} nie znalezione." });
            }

            var normalized = request.Name.Trim();

            if (cities.Any(c => c.Id != id && c.Name.Equals(normalized, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Conflict(new { message = $"Miasto '{normalized}' już istnieje." });
            }

            city.Name = normalized;
            return Results.Ok(city);
        })
        .WithName("UpdateCity")
        .WithOpenApi();

        app.MapDelete("/cities/{id:int}", (int id) =>
        {
            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return Results.NotFound(new { message = $"Miasto o ID {id} nie znalezione." });
            }

            cities.Remove(city);
            return Results.Ok(new { message = $"Usunięto miasto: {city.Name}" });
        })
        .WithName("DeleteCity")
        .WithOpenApi();

        return app;
    }
}
