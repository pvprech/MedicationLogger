using System.ComponentModel.DataAnnotations;

namespace MedikamentenLogger.Api.Dtos;

public record UpdateEntryRatingDto(
    [Required] int StarRatingId,
    [Required][Range(0, 10)] byte Rating
);