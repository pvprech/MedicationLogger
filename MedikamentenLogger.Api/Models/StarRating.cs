namespace MedikamentenLogger.Api.Models;

public class StarRating
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public required string Name { get; set; }
    public int DisplayOrder { get; set; }
}
