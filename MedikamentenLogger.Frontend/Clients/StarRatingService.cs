using MedikamentenLogger.Frontend.Models;

namespace MedikamentenLogger.Frontend.Clients;

public class StarRatingService
{
    // every list in this project got created by an ai because im way to lazy
    private readonly List<StarRating> starRatings =
    [
        new StarRating { Id = 1, Name = "Appetitlosigkeit", Rating = 0, DisplayOrder = 1 },
        new StarRating { Id = 2, Name = "Mundtrockenheit", Rating = 0, DisplayOrder = 2 },
        new StarRating { Id = 3, Name = "Einschlafprobleme / Schlafstörungen", Rating = 0, DisplayOrder = 3 },
        new StarRating { Id = 4, Name = "Kopfschmerzen", Rating = 0, DisplayOrder = 4 },
        new StarRating { Id = 5, Name = "Rebound-Crash (je Höher desto schlimmer)", Rating = 0, DisplayOrder = 5 },
        new StarRating { Id = 6, Name = "Subjektives Herzklopfen / Ängstlichkeit", Rating = 0, DisplayOrder = 6 },
        new StarRating { Id = 7, Name = "Gereiztheit / Emotionale Abflachung", Rating = 0, DisplayOrder = 7 },
        new StarRating { Id = 8, Name = "Magen-Darm-Beschwerden / Übelkeit", Rating = 0, DisplayOrder = 8 },

        new StarRating { Id = 9, Name = "Verbesserter Fokus & Konzentration", Rating = 0, DisplayOrder = 9 },
        new StarRating { Id = 10, Name = "Bessere Impulskontrolle (Innehalten)", Rating = 0, DisplayOrder = 10 },
        new StarRating { Id = 11, Name = "Steigerung von Stimmung & Antrieb", Rating = 0, DisplayOrder = 11 },
        new StarRating { Id = 12, Name = "Reduktion der inneren Unruhe / Getriebenheit", Rating = 0, DisplayOrder = 12 },
        new StarRating { Id = 13, Name = "Bessere Emotionale Regulation", Rating = 0, DisplayOrder = 13 },
        new StarRating { Id = 14, Name = "Leichtere Handlungsplanung (Exekutive Funktion)", Rating = 0, DisplayOrder = 14 },
        new StarRating { Id = 15, Name = "Weniger gedankliches Abschweifen (Mind-Wandering)", Rating = 0, DisplayOrder = 15 }
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