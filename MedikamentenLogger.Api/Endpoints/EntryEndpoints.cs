using MedikamentenLogger.Api.Data;
using MedikamentenLogger.Api.Dtos.EntryDtos;
using MedikamentenLogger.Api.Dtos.RatingDtos;
using MedikamentenLogger.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedikamentenLogger.Api.Endpoints;

public static class EntryEndpoints
{
    // Method is creating the full frontend instance of the entry object
    // This is only needed by creating an new object because the Frontend is lazy loading
    private static async Task<EntryDetailsDto?> GetEntryDetailsByIdAsync(int entryId, MLContext dbContext)
    {
        // Extracting the Ratings for the entries via join 
        // because the database does not contain Lists => I always use relational Databases because its more fun
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

        // "Building" the Frontend instance
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
        // First Dto the Frontend pulls. Used in a calender or similar => always a List
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
        // Second Dto the Frontend pulls. Used when the entry is opened in a page => always one
        group.MapGet("/openedEntry/{entryId}", async (int entryId, MLContext dbContext) =>
        {
            // Extracting the costum ratings that are expected from the frontend but stored relational
            List<EntryRatingDto> ratingDtos = await dbContext.EntryRatings
                .Where(rating => rating.EntryId == entryId)
                    .Select(rating => new EntryRatingDto(
                        dbContext.StarRatings
                            .FirstOrDefault(starRating => starRating.Id == rating.StarRatingId)!.Name,
                        rating.Rating,
                        rating.DisplayOrder
                    ))
                    .ToListAsync();

            // "Building" the instance 
            // => Ids and Date is not needed because it already got it from the first Dto
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
        // This GET request is ONLY required for the POST request because the Frontend wants to 
        // instantly open the entry after creation instead of lazy loading it (horrible ux + more requests)
        group.MapGet("/entryDetails/{entryId}", async (int entryId, MLContext dbContext) =>
        {
            var entryDetails = await GetEntryDetailsByIdAsync(entryId, dbContext);
            return entryDetails is not null ? Results.Ok(entryDetails) : Results.NotFound();
        }).WithName(GetEntryEndpointName);



        // POST /entries
        // POST ONLY the ENTRY the ratings are determined from the MedicationId and are coming from the StarRatinEndpoint
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

            // Get the full instance for instant showcase in frontend
            EntryDetailsDto responseDto = (await GetEntryDetailsByIdAsync(entry.Id, dbContext))!; // CANT be null if it is null the db is corrupted
            return Results.CreatedAtRoute(GetEntryEndpointName, new { entryId = entry.Id }, responseDto);
        });



        // PUT /entries/{id}
        group.MapPut("/{entryId}", async (int entryId, UpdateEntryDto updatedEntry, MLContext dbContext) =>
        {
            var entry = await dbContext.Entries.FindAsync(entryId);
            if (entry is null) return Results.NotFound();

            // User should not be able to update everything i.e. the Date (auto created)
            entry.GeneralEffectiveness = updatedEntry.GeneralEffectiveness;
            entry.GeneralSideEffects = updatedEntry.GeneralSideEffects;
            entry.UserNote = updatedEntry.UserNote;

            // Putting the ratings from the dto in an dictionary to "extract" the ids for changing the ones in the db
            var ratingsUpdateDict = updatedEntry.Ratings
                .ToDictionary(r => r.StarRatingId, r => r.Rating);

            // Pulling from the db using the dictionary and putting it in a List for changing
            List<EntryRating> ratings = await dbContext.EntryRatings
                .Where(rating => rating.EntryId == entryId && ratingsUpdateDict.Keys.Contains(rating.StarRatingId))
                .ToListAsync();

            // Changing the Ratings using the dictionary
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
        // Delete everything dont tell frontend the frontend does not need to know if it existed
        group.MapDelete("/{entryId}", async (int entryId, MLContext dbContext) =>
        {
            await dbContext.Entries
                .Where(entry => entry.Id == entryId)
                    .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
