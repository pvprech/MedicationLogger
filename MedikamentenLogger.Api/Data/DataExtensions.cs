using System.Text.Json;
using MedikamentenLogger.Api.Dtos.SpecialDtos;
using MedikamentenLogger.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedikamentenLogger.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<MLContext>();

        dbContext.Database.Migrate();
    }

    public static void SeedMlDb(this WebApplicationBuilder builder)
    {
        // Seeding
        var connString = builder.Configuration.GetConnectionString("MedicationLogger");
        builder.Services.AddSqlite<MLContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                var mlContext = (MLContext)context;

                if (mlContext.Set<StarRating>().Any())
                {
                    return;
                }

                var jsonPath = Path.Combine(AppContext.BaseDirectory, "seed.json");
                if (!File.Exists(jsonPath))
                {
                    return;
                }

                var json = File.ReadAllText(jsonPath);

                var optionsJson = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var seedData = JsonSerializer.Deserialize<SeedDataDto>(json, optionsJson);

                if (seedData != null)
                {
                    mlContext.Set<StarRating>().AddRange(seedData.StarRatings);
                    mlContext.Set<Entry>().AddRange(seedData.Entries);
                    mlContext.Set<EntryRating>().AddRange(seedData.EntryRatings);

                    mlContext.SaveChanges();
                }
            })
        );
    }
}
