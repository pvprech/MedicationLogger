using MedikamentenLogger.Shared.Dtos.RatingDtos;

namespace MedikamentenLogger.Shared.Dtos.EntryDtos;

public record EntryDetailsDto(
    int Id,
    int MedicationId,
    DateOnly Date,
    byte GeneralEffectiveness,
    byte GeneralSideEffects,
    string UserNote,
    List<StarRatingDetailsDto> SpecificRatings
);
