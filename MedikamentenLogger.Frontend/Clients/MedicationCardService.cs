using MedikamentenLogger.Frontend.Models;

namespace MedikamentenLogger.Frontend.Clients;

public class MedicationCardService
{
    private readonly List<MedicationCard> cards =
    [
        new MedicationCard {
            Id = 1,
            Name = "Kinecteen 18mg",
            Description = "ADHS-Medikation mit 18mg Methylphenidathydrochlorid als aktiver Wirkstoff." +
                            "Plateu-Präperat mit einer erwarteten Wirkdauer von 10h.",
            ImagePath = "images/Kinecteen.webp"
        },

        new MedicationCard
        {
            Id = 2,
            Name = "Ritalin",
            Description = "Methylphenidat-Präparat zur Behandlung von ADHS. Wirkt unretardiert mit kürzerer Wirkdauer und schnellerem Wirkungseintritt.",
            ImagePath = "images/Ritalin.webp"
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