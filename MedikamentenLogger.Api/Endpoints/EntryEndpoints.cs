using MedikamentenLogger.Api.Data;
using MedikamentenLogger.Api.Dtos.EntryDtos;
using MedikamentenLogger.Api.Dtos.RatingDtos;
using MedikamentenLogger.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedikamentenLogger.Api.Endpoints;

public static class EntryEndpoints
{
    private static async Task<EntryDetailsDto?> GetEntryDetailsByIdAsync(int entryId, MLContext dbContext)
    {
        List<StarRatingDetailsDto> ratings = await dbContext.EntryRatings.Where(rating => rating.EntryId == entryId)
                .Join(
                    dbContext.StarRatings,
                    rating => rating.StarRatingId,
                    starRating => starRating.Id,
                    (rating, starRating) => new StarRatingDetailsDto(
                        starRating.Id,
                        starRating.Name,
                        rating.Rating,
                        rating.DisplayOrder
                    )
                ).ToListAsync();

        EntryDetailsDto? entryDetails = await dbContext.Entries
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
                .FirstOrDefaultAsync();

        return entryDetails;
    }


    public static void MapEntryEndpoints(this WebApplication app)
    {
        const string GetEntryEndpointName = "GetEntry";

        var group = app.MapGroup("/entries");



        // GET for PageEntryDto /entries/pageEntries{medicationId}
        group.MapGet("/pageEntries/{medicationId}", async (int medicationId, MLContext dbContext) =>
        {
            List<PageEntryDto> pageEntryDtos = await dbContext.Entries.Where(entry => entry.MedicationId == medicationId)
                .Select(entry => new PageEntryDto
                (
                    entry.Id,
                    entry.MedicationId,
                    entry.Date
                ))
                .ToListAsync();

            return Results.Ok(pageEntryDtos);
        });



        // GET for OpenedEntryDto /entries/openedEntry/{entryId}
        group.MapGet("/openedEntry/{entryId}", async (int entryId, MLContext dbContext) =>
        {
            List<EntryRatingDto> ratingDtos = await dbContext.EntryRatings
                .Where(rating => rating.EntryId == entryId)
                    .Select(rating => new EntryRatingDto(
                        dbContext.StarRatings
                            .FirstOrDefault(starRating => starRating.Id == rating.StarRatingId)!.Name,
                        rating.Rating,
                        rating.DisplayOrder
                    ))
                    .ToListAsync();

            OpenedEntryDto? openedEntryDto = await dbContext.Entries
                .Where(entry => entry.Id == entryId)
                    .Select(entry => new OpenedEntryDto(
                        entry.GeneralEffectiveness,
                        entry.GeneralSideEffects,
                        entry.UserNote,
                        ratingDtos
                    ))
                    .FirstOrDefaultAsync();

            return openedEntryDto is not null ? Results.Ok(openedEntryDto) : Results.NotFound();
        });



        // GET for EntryDetailsDto /entries/entryDetails/{entryId}
        group.MapGet("/entryDetails/{entryId}", async (int entryId, MLContext dbContext) =>
        {
            var entryDetails = await GetEntryDetailsByIdAsync(entryId, dbContext);
            return entryDetails is not null ? Results.Ok(entryDetails) : Results.NotFound();
        }).WithName(GetEntryEndpointName);



        // POST /entries
        group.MapPost("/", async (CreateEntryDto newEntry, MLContext dbContext) =>
        {
            Entry entry = new()
            {
                MedicationId = newEntry.MedicationId,
                Date = newEntry.Date,
                GeneralEffectiveness = newEntry.GenrealEffectiveness,
                GeneralSideEffects = newEntry.GeneralSideEffects,
                UserNote = newEntry.UserNote
            };

            dbContext.Entries.Add(entry);
            await dbContext.SaveChangesAsync();

            EntryDetailsDto responseDto = (await GetEntryDetailsByIdAsync(entry.Id, dbContext))!;
            return Results.CreatedAtRoute(GetEntryEndpointName, new { entryId = entry.Id }, responseDto);
        });



        // PUT /entries/{id}
        group.MapPut("/{entryId}", async (int entryId, UpdateEntryDto updatedEntry, MLContext dbContext) =>
        {
            var entry = await dbContext.Entries.FindAsync(entryId);
            if (entry is null) return Results.NotFound();

            entry.GeneralEffectiveness = updatedEntry.GeneralEffectiveness;
            entry.GeneralSideEffects = updatedEntry.GeneralSideEffects;
            entry.UserNote = updatedEntry.UserNote;


            var ratingsUpdateDict = updatedEntry.Ratings
                .ToDictionary(r => r.StarRatingId, r => r.Rating);

            List<EntryRating> ratings = await dbContext.EntryRatings
                .Where(rating => rating.EntryId == entryId && ratingsUpdateDict.Keys.Contains(rating.StarRatingId))
                .ToListAsync();

            foreach (var rating in ratings)
            {
                if (ratingsUpdateDict.TryGetValue(rating.StarRatingId, out var newRatingValue))
                {
                    rating.Rating = newRatingValue;
                }
            }

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });



        // DELETE /entries/{entryId}
        group.MapDelete("/{entryId}", async (int entryId, MLContext dbContext) =>
        {
            await dbContext.Entries
                .Where(entry => entry.Id == entryId)
                    .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
