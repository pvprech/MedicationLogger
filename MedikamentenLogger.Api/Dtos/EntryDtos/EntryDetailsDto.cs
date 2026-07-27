using MedikamentenLogger.Api.Dtos.RatingDtos;

namespace MedikamentenLogger.Api.Dtos.EntryDtos;

public record EntryDetailsDto(
    int Id,
    int MedicationId,
    DateOnly Date,
    byte GeneralEffectiveness,
    byte GeneralSideEffects,
    string UserNote,
    List<StarRatingDetailsDto> SpecificRatings
);
