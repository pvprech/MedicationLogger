using System.Text.Json;
using MedikamentenLogger.Api.Data;
using MedikamentenLogger.Api.Dtos.SpecialDtos;
using MedikamentenLogger.Api.Endpoints;
using MedikamentenLogger.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();


// Seeding
var connString = "Data Source=MedicationLogger.db";
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



var app = builder.Build();

app.MapEntryEndpoints();
app.MapStarRatingEndpoints();
app.MapMedicationEndpoints();

app.MigrateDb();

app.Run();
