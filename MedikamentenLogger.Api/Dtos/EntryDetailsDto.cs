using MedikamentenLogger.Api.Models;

namespace MedikamentenLogger.Api.Dtos;

public record EntryDetailsDto(
    int Id,
    int MedicationId,
    DateOnly Date,
    byte GeneralEffectiveness,
    byte GeneralSideEffects,
    string UserNote,
    List<StarRatingDetailsDto> SpecificRatings
);
