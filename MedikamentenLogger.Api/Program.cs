using MedikamentenLogger.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapEntryEndpoints();
app.MapStarRatingEndpoints();
app.MapMedicationEndpoints();

app.Run();
