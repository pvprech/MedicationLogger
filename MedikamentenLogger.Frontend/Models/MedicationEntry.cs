namespace MedikamentenLogger.Frontend.Models;

public class MedicationEntry
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public byte GeneralEffectiveness { get; set; }
    public byte GeneralSideEffects { get; set; }
    public List<StarRating> SpecificRatings { get; set; } = [];
    public required string UserNote { get; set; }
    public List<string> ImagePaths { get; set; } = [];
}