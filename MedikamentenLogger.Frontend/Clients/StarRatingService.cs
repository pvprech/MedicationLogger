using MedikamentenLogger.Frontend.Models;

namespace MedikamentenLogger.Frontend.Clients;

public class StarRatingService
{
    private readonly List<StarRating> starRatings =
    [
        new StarRating
        {
            Id = 1,
            Name = "Fokus & Konzentration",
            Rating = 4,
            DisplayOrder = 1
        },
        new StarRating
        {
            Id = 2,
            Name = "Stimmung / Antrieb",
            Rating = 3,
            DisplayOrder = 2
        },

        new StarRating
        {
            Id = 3,
            Name = "Mundtrockenheit",
            Rating = 2,
            DisplayOrder = 3
        },
        new StarRating
        {
            Id = 4,
            Name = "Appetitlosigkeit",
            Rating = 4,
            DisplayOrder = 4
        },
        new StarRating
        {
            Id = 5,
            Name = "Kopfschmerzen",
            Rating = 1,
            DisplayOrder = 5
        },
        new StarRating
        {
            Id = 6,
            Name = "Schlafstörungen",
            Rating = 1,
            DisplayOrder = 6
        }
    ];

    public StarRating[] GetStarRatings()
    {
        return [.. starRatings];
    }

    public StarRating? GetStarRatingById(int id)
    {
        return starRatings.FirstOrDefault(x => x.Id == id);
    }

    public void AddStarRating(StarRating starRating)
    {
        starRating.Id = starRatings.Count > 0 ? starRatings.Max(x => x.Id) + 1 : 1;
        starRatings.Add(starRating);
    }

    public void DeleteStarRatingById(int id)
    {
        starRatings.RemoveAll(x => x.Id == id);
    }

    // ONLY FOR UI TESTING
    public StarRating GetRatingForEntry(int id, byte ratingValue)
    {
        var template = GetStarRatingById(id)
            ?? throw new InvalidOperationException($"Rating mit ID {id} nicht gefunden.");

        return new StarRating
        {
            Id = template.Id,
            Name = template.Name,
            Rating = ratingValue,
            DisplayOrder = template.DisplayOrder
        };
    }
}