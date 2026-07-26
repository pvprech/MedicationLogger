namespace MedikamentenLogger.Api.Models;

public class EntryRating
{
    public int StarRatingId { get; set; }
    public int EntryId { get; set; }
    public int DisplayOrder { get; set; }
    public byte Rating { get; set; }
}
