using MedikamentenLogger.Api.Data;
using MedikamentenLogger.Api.Endpoints;

// Development only:
// - Remove database seeding.
// - Remove automatic migrations on startup.
// - Register the production SQLite database.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

//var connString = builder.Configuration.GetConnectionString("MedicationLogger");
//builder.Services.AddSqlite<MLContext>(connString);

// Seeding remove before production
builder.SeedMlDb();

var app = builder.Build();

app.MapEntryEndpoints();
app.MapStarRatingEndpoints();
app.MapMedicationEndpoints();

// Migrating db at every start remove before production
app.MigrateDb();

app.Run();
