using System.ComponentModel.DataAnnotations;

namespace MedikamentenLogger.Shared.Dtos.RatingDtos;

public record UpdateEntryRatingDto(
    [Required] int StarRatingId,
    [Required][Range(0, 10)] byte Rating
);