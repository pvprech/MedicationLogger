namespace MedikamentenLogger.Shared.Dtos.RatingDtos;

public record StarRatingDetailsDto(
    int Id,
    string Name,
    byte Rating,
    int DisplayOrder
);
