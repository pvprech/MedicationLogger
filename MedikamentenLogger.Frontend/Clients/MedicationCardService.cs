using MedikamentenLogger.Frontend.Models;

namespace MedikamentenLogger.Frontend.Clients;

public class MedicationCardService(StarRatingService starRatingService)
{

    private readonly List<MedicationCard> cards =
    [
        new MedicationCard {
            Id = 1,
            Name = "Kinecteen 18mg",
            Description = "ADHS-Medikation mit 18mg Methylphenidathydrochlorid als aktiver Wirkstoff." +
                            "Plateu-Präperat mit einer erwarteten Wirkdauer von 10h.",
            ImagePath = "images/Kinecteen.webp",
            StarRatings = [.. starRatingService.GetStarRatings()]
        },

        new MedicationCard
        {
            Id = 2,
            Name = "Kinecteen 27mg",
            Description = "ADHS-Medikation mit 27mg Methylphenidathydrochlorid als aktiver Wirkstoff." +
                            "Plateu-Präperat mit einer erwarteten Wirkdauer von 10h.",
            ImagePath = "images/Ritalin.webp",
            StarRatings = [.. starRatingService.GetStarRatings()]
        },

        new MedicationCard
        {
            Id = 3,
            Name = "Elvanse",
            Description = "ADHS-Medikation mit Lisdexamfetamin als Wirkstoff. Wird im Körper zu Dexamfetamin umgewandelt und wirkt über mehrere Stunden.",
            ImagePath = "images/Elvanse.webp"
        },

        new MedicationCard
        {
            Id = 4,
            Name = "Ibuprofen",
            Description = "Schmerz- und entzündungshemmendes Medikament aus der Gruppe der nichtsteroidalen Antirheumatika.",
            ImagePath = "images/Ibuprofen.webp"
        },

        new MedicationCard
        {
            Id = 5,
            Name = "Magnesiumcitrat",
            Description = "Nahrungsergänzungsmittel mit Magnesium zur Unterstützung der normalen Muskelfunktion und des Elektrolythaushalts.",
            ImagePath = "images/Magnesiumcitrat.webp"
        }
    ];

    public MedicationCard[] GetMedicationCards()
    {
        return [.. cards];
    }

    public MedicationCard? GetMedicationCardByID(int id)
    {
        return cards.FirstOrDefault(x => x.Id == id);
    }

    public void AddMedicationCard(MedicationCard card)
    {
        card.Id = cards.Count > 0 ? cards.Max(x => x.Id) + 1 : 1;
        cards.Add(card);
    }

    public void DeleteMedicationCardById(int id)
    {
        cards.RemoveAll(x => x.Id == id);
    }
}