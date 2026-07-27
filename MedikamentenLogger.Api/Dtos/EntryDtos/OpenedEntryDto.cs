using MedikamentenLogger.Api.Dtos.RatingDtos;

namespace MedikamentenLogger.Api.Dtos.EntryDtos;

public record OpenedEntryDto(
    byte GeneralEffectiveness,
    byte GeneralSideEffects,
    string UserNote,
    List<EntryRatingDto> Ratings
);
