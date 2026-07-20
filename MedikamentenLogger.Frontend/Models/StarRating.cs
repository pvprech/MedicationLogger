namespace MedikamentenLogger.Frontend.Models;

public class StarRating
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public byte? Rating { get; set; }
    public int DisplayOrder { get; set; }
}