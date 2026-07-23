using MedikamentenLogger.Frontend.Models;

namespace MedikamentenLogger.Frontend.Clients;

public class MedicationEntryService
{
    private readonly StarRatingService ratingService = new();

    private readonly List<MedicationEntry> entries;

    public MedicationEntryService()
    {
        entries =
        [
            new MedicationEntry
            {
                Id = 1,
                MedicationId = 1,
                Date = new DateOnly(2026, 7, 20),
                GeneralEffectiveness = 4,
                GeneralSideEffects = 2,
                SpecificRatings =
                [
                    ratingService.GetRatingForEntry(id: 1, ratingValue: 4),
                    ratingService.GetRatingForEntry(id: 3, ratingValue: 2)
                ],
                UserNote = "Gute Wirkung im Alltag. Leichte Mundtrockenheit gegen Nachmittag.",
                ImagePaths = ["uploads/2026-07-20_rezept.jpg"]
            },
            new MedicationEntry
            {
                Id = 2,
                MedicationId = 1,
                Date = new DateOnly(2026, 7, 19),
                GeneralEffectiveness = 2,
                GeneralSideEffects = 4,
                SpecificRatings =
                [
                    ratingService.GetRatingForEntry(id: 1, ratingValue: 2),
                    ratingService.GetRatingForEntry(id: 4, ratingValue: 5),
                    ratingService.GetRatingForEntry(id: 5, ratingValue: 3)
                ],
                UserNote = "Wirkung eher schwach, dafür relativ starke Appetitlosigkeit mittags.",
                ImagePaths = []
            },
            new MedicationEntry
            {
                Id = 3,
                MedicationId = 1,
                Date = new DateOnly(2026, 7, 17),
                GeneralEffectiveness = 3,
                GeneralSideEffects = 3,
                SpecificRatings =
                [
                    ratingService.GetRatingForEntry(id: 1, ratingValue: 3),
                    ratingService.GetRatingForEntry(id: 3, ratingValue: 3),
                    ratingService.GetRatingForEntry(id: 6, ratingValue: 2)
                ],
                UserNote = "Mittelmäßiger Tag. Gegen Abend etwas unruhig geschlafen.",
                ImagePaths = []
            },
            new MedicationEntry
            {
                Id = 4,
                MedicationId = 1,
                Date = new DateOnly(2026, 7, 16),
                GeneralEffectiveness = 5,
                GeneralSideEffects = 1,
                SpecificRatings =
                [
                    ratingService.GetRatingForEntry(id: 1, ratingValue: 5),
                    ratingService.GetRatingForEntry(id: 2, ratingValue: 5),
                    ratingService.GetRatingForEntry(id: 3, ratingValue: 1)
                ],
                UserNote = "Sehr produktiver Arbeitstag, volle Konzentration ohne Ablenkung.",
                ImagePaths = ["uploads/2026-07-16_fokus_notiz.jpg"]
            },
            new MedicationEntry
            {
                Id = 5,
                MedicationId = 1,
                Date = new DateOnly(2026, 7, 15),
                GeneralEffectiveness = 1,
                GeneralSideEffects = 5,
                SpecificRatings =
                [
                    ratingService.GetRatingForEntry(id: 1, ratingValue: 1),
                    ratingService.GetRatingForEntry(id: 4, ratingValue: 4),
                    ratingService.GetRatingForEntry(id: 5, ratingValue: 5)
                ],
                UserNote = "Starke Kopfschmerzen ab mittags. Kaum Wirkung gespürt.",
                ImagePaths = []
            },
            new MedicationEntry
            {
                Id = 6,
                MedicationId = 1,
                Date = new DateOnly(2026, 7, 14),
                GeneralEffectiveness = 4,
                GeneralSideEffects = 2,
                SpecificRatings =
                [
                    ratingService.GetRatingForEntry(id: 1, ratingValue: 4),
                    ratingService.GetRatingForEntry(id: 2, ratingValue: 4),
                    ratingService.GetRatingForEntry(id: 6, ratingValue: 2)
                ],
                UserNote = "Guter Antrieb, Aufgaben zügig abgearbeitet.",
                ImagePaths = []
            },
            new MedicationEntry
            {
                Id = 7,
                MedicationId = 1,
                Date = new DateOnly(2026, 7, 13),
                GeneralEffectiveness = 2,
                GeneralSideEffects = 2,
                SpecificRatings =
                [
                    ratingService.GetRatingForEntry(id: 1, ratingValue: 2),
                    ratingService.GetRatingForEntry(id: 3, ratingValue: 2),
                    ratingService.GetRatingForEntry(id: 4, ratingValue: 3)
                ],
                UserNote = "Einnahme etwas zu spät erfolgt, Dosis wirkte erst am Nachmittag leicht.",
                ImagePaths = ["uploads/2026-07-13_uhrzeit.jpg"]
            }
        ];
    }

    public MedicationEntry[] GetMedicationEntriesByMedicationId(int id)
    {
        return [.. entries.Where(x => x.MedicationId == id).OrderByDescending(x => x.Date)];
    }

    public MedicationEntry? GetMedicationEntryByID(int id)
    {
        return entries.FirstOrDefault(x => x.Id == id);
    }

    public int AddMedicationEntry(MedicationEntry entry)
    {
        var existingEntry = entries.FirstOrDefault(x => x.Date == entry.Date);
        if (existingEntry != null)
        {
            return existingEntry.Id;
        }

        entry.Id = entries.Count > 0 ? entries.Max(x => x.Id) + 1 : 1;
        entries.Add(entry);
        return entry.Id;
    }

    public void DeleteMedicationEntryById(int id)
    {
        entries.RemoveAll(x => x.Id == id);
    }

    public void UpdateMedicationEntry(MedicationEntry entry)
    {
        int index = entries.FindIndex(x => x.Id == entry.Id);
        entries[index] = entry;
    }
}