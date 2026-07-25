using MedikamentenLogger.Api.Dtos;

namespace MedikamentenLogger.Api.Endpoints;

public static class EntryEndpoints
{
    public static void MapEntryEndpoints(this WebApplication app)
    {
        const string GetEntryEndpointName = "GetEntry";

        var group = app.MapGroup("/entries");

        List<EntryDto> entries =
        [
            new(
                Id: 1,
                MedicationId: 101,
                Date: new DateOnly(2026, 7, 24),
                GenrealEffectiveness: 8,
                GeneralSideEffects: 2,
                UserNote: "Wirkung setzt nach ca. 30 Minuten ein. Leichte Trockenheit im Mund, aber insgesamt gut verträglich."
            ),

            new(
                Id: 2,
                MedicationId: 102,
                Date: new DateOnly(2026, 7, 25),
                GenrealEffectiveness: 3,
                GeneralSideEffects: 9,
                UserNote: "Starke Müdigkeit und Schwindel. Kaum Schmerzlinderung spürbar."
            ),

            new(
                Id: 3,
                MedicationId: 101,
                Date: new DateOnly(2026, 7, 26),
                GenrealEffectiveness: 5,
                GeneralSideEffects: 0,
                UserNote: "Keine Auffälligkeiten."
            )
        ];

        // GET /entries
        group.MapGet("/", () => entries);

        // GET /entries/{id}
        group.MapGet("/{id}", (int id) => entries.Find(entry => entry.Id == id))
            .WithName(GetEntryEndpointName);

        // POST /entries
        group.MapPost("/", (CreateEntryDto newEntry) =>
        {
            EntryDto entry = new(
                entries.Count + 1,
                newEntry.MedicationId,
                newEntry.Date,
                newEntry.GenrealEffectiveness,
                newEntry.GeneralSideEffects,
                newEntry.UserNote
            );

            entries.Add(entry);
            return Results.CreatedAtRoute(GetEntryEndpointName, new { id = entry.Id }, entry);
        });

        // PUT /entries/{id}
        group.MapPut("/{id}", (int id, UpdateEntryDto updatedEntry) =>
        {
            int index = entries.FindIndex(entry => entry.Id == id);

            entries[index] = new(
                id,
                entries[index].MedicationId,
                updatedEntry.Date,
                updatedEntry.GeneralEffectiveness,
                updatedEntry.GeneralSideEffects,
                updatedEntry.UserNote
            );

            return Results.NoContent();
        });

        // DELETE /entries/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            entries.RemoveAll(entry => entry.Id == id);

            return Results.NoContent();
        });
    }
}
