using System.Reflection.Metadata.Ecma335;
using MedikamentenLogger.Api.Dtos;
using MedikamentenLogger.Api.Models;

namespace MedikamentenLogger.Api.Endpoints;

public static class EntryEndpoints
{
    static readonly List<StarRating> starRatings =
        [
            new() {
                Id = 1,
                MedicationId = 1,
                Name = "Verträglichkeit",
                DisplayOrder = 1
            },
            new() {
                Id = 2,
                MedicationId = 1,
                Name = "Wirksamkeit",
                DisplayOrder = 2
            },
            new() {
                Id = 3,
                MedicationId = 2,
                Name = "Einnahmeaufwand",
                DisplayOrder = 3
            }
        ];

    static readonly List<EntryRating> entryRatings =
    [
        new() {
                StarRatingId = 1,
                EntryId = 1,
                DisplayOrder = 1,
                Rating = 5
            },
            new() {
                StarRatingId = 2,
                EntryId = 1,
                DisplayOrder = 2,
                Rating = 4
            },
            new() {
                StarRatingId = 3,
                EntryId = 2,
                DisplayOrder = 3,
                Rating = 2
            }
    ];

    static readonly List<Entry> entries =
    [
        new() {
                Id = 1,
                MedicationId = 1,
                Date = DateOnly.FromDateTime(DateTime.Today),
                GeneralEffectiveness = 4,
                GeneralSideEffects = 1,
                UserNote = "Gute Wirkung im Laufe des Vormittags. Leichte Müdigkeit nach der Einnahme."
            },
            new() {
                Id = 2,
                MedicationId = 1,
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                GeneralEffectiveness = 5,
                GeneralSideEffects = 0,
                UserNote = "Keine Nebenwirkungen gespürt, Symptome komplett verschwunden."
            },
            new() {
                Id = 3,
                MedicationId = 2,
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                GeneralEffectiveness = 2,
                GeneralSideEffects = 3,
                UserNote = "Später eingenommen als sonst. Magenstechen am Nachmittag."
            }
    ];

    private static EntryDetailsDto? GetEntryDetailsById(int entryId)
    {
        List<StarRatingDetailsDto> ratings = [.. entryRatings.Where(rating => rating.EntryId == entryId)
                .Join(
                    starRatings,
                    rating => rating.StarRatingId,
                    starRating => starRating.Id,
                    (rating, starRating) => new StarRatingDetailsDto(
                        starRating.Id,
                        starRating.Name,
                        rating.Rating,
                        rating.DisplayOrder
                    )
                )];

        EntryDetailsDto? entryDetails = entries
            .Where(entry => entry.Id == entryId)
                .Select(entry => new EntryDetailsDto(
                    entry.Id,
                    entry.MedicationId,
                    entry.Date,
                    entry.GeneralEffectiveness,
                    entry.GeneralSideEffects,
                    entry.UserNote,
                    ratings
                ))
                .FirstOrDefault();

        return entryDetails;
    }


    public static void MapEntryEndpoints(this WebApplication app)
    {
        const string GetEntryEndpointName = "GetEntry";

        var group = app.MapGroup("/entries");



        // GET for PageEntryDto /entries/pageEntries{medicationId}
        group.MapGet("/pageEntries/{medicationId}", (int medicationId) =>
        {
            List<PageEntryDto> pageEntryDtos = [.. entries.Where(entry => entry.MedicationId == medicationId)
                .Select(entry => new PageEntryDto
                (
                    entry.Id,
                    entry.MedicationId,
                    entry.Date
                ))];

            return Results.Ok(pageEntryDtos);
        });



        // GET for OpenedEntryDto /entries/openedEntry/{entryId}
        group.MapGet("/openedEntry/{entryId}", (int entryId) =>
        {
            List<EntryRatingDto> ratingDtos = [.. entryRatings
                .Where(rating => rating.EntryId == entryId)
                    .Select(rating => new EntryRatingDto(
                        starRatings.FirstOrDefault(starRating => starRating.Id == rating.StarRatingId)!.Name,
                        rating.Rating,
                        rating.DisplayOrder
                    ))];

            OpenedEntryDto? openedEntryDto = entries
                .Where(entry => entry.Id == entryId)
                    .Select(entry => new OpenedEntryDto(
                        entry.GeneralEffectiveness,
                        entry.GeneralSideEffects,
                        entry.UserNote,
                        ratingDtos
                    ))
                    .FirstOrDefault();

            return openedEntryDto is not null ? Results.Ok(openedEntryDto) : Results.NotFound();
        });



        // GET for EntryDetailsDto /entries/entryDetails/{entryId}
        group.MapGet("/entryDetails/{entryId}", (int entryId) =>
        {
            var entryDetails = GetEntryDetailsById(entryId);
            return entryDetails is not null ? Results.Ok(entryDetails) : Results.NotFound();
        }).WithName(GetEntryEndpointName);



        // POST /entries
        group.MapPost("/", (CreateEntryDto newEntry) =>
        {
            Entry entry = new()
            {
                Id = entries.Count + 1,
                MedicationId = newEntry.MedicationId,
                Date = newEntry.Date,
                GeneralEffectiveness = newEntry.GenrealEffectiveness,
                GeneralSideEffects = newEntry.GeneralSideEffects,
                UserNote = newEntry.UserNote
            };
            entries.Add(entry);

            EntryDetailsDto responseDto = GetEntryDetailsById(entry.Id)!;
            return Results.CreatedAtRoute(GetEntryEndpointName, new { entryId = entry.Id }, responseDto);
        });



        // PUT /entries/{id}
        group.MapPut("/{entryId}", (int entryId, UpdateEntryDto updatedEntry) =>
        {
            int index = entries.FindIndex(entry => entry.Id == entryId);
            if (index == -1) return Results.NotFound();

            entries[index] = new()
            {
                GeneralEffectiveness = updatedEntry.GeneralEffectiveness,
                GeneralSideEffects = updatedEntry.GeneralSideEffects,
                UserNote = updatedEntry.UserNote,

                // old values
                Id = entries[index].Id,
                MedicationId = entries[index].MedicationId,
                Date = entries[index].Date
            };

            var allowedIds = updatedEntry.Ratings.Select(rating => rating.StarRatingId).ToHashSet();

            List<EntryRating> ratings = [.. entryRatings
                .Where(rating => rating.EntryId == entryId)
                    .Where(rating => allowedIds
                        .Contains(rating.StarRatingId))];

            foreach (var rating in ratings)
            {
                rating.Rating = updatedEntry.Ratings.First(r => r.StarRatingId == rating.StarRatingId).Rating;
            }

            return Results.NoContent();
        });



        // DELETE /entries/{entryId}
        group.MapDelete("/{entryId}", (int entryId) =>
        {
            var entry = entries.FirstOrDefault(e => e.Id == entryId);
            if (entry is null) return Results.NotFound();

            entryRatings.RemoveAll(rating => rating.EntryId == entryId);
            entries.Remove(entry);

            return Results.NoContent();
        });
    }
}
