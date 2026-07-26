namespace MedikamentenLogger.Api.Dtos;

public record StarRatingDetailsDto(
    int Id,
    string Name,
    byte Rating,
    int DisplayOrder
);
