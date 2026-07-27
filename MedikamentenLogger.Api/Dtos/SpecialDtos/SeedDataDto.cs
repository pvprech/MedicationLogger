using MedikamentenLogger.Api.Models;

namespace MedikamentenLogger.Api.Dtos.SpecialDtos;

public class SeedDataDto
{
    public List<StarRating> StarRatings { get; set; } = [];
    public List<Entry> Entries { get; set; } = [];
    public List<EntryRating> EntryRatings { get; set; } = [];
}
