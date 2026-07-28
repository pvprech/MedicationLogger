using MedikamentenLogger.Api.Data;
using MedikamentenLogger.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.SeedMlDb();

var app = builder.Build();

app.MapEntryEndpoints();
app.MapStarRatingEndpoints();
app.MapMedicationEndpoints();

app.MigrateDb();

app.Run();
