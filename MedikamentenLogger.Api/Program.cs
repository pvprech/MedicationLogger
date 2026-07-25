using MedikamentenLogger.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Jason ist ein Hurensohn!");

app.MapEntryEndpoints();

app.Run();
