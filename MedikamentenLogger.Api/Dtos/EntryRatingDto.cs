using System.ComponentModel.DataAnnotations;

namespace MedikamentenLogger.Api.Dtos;

public record EntryRatingDto(
    [Required] string Name,
    [Required][Range(0, 10)] byte Rating,
    int DisplayOrder
);
