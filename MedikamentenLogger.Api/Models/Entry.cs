namespace MedikamentenLogger.Api.Models;

public class Entry
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public DateOnly Date { get; set; }
    public byte GeneralEffectiveness { get; set; }
    public byte GeneralSideEffects { get; set; }
    public required string UserNote { get; set; }
}
