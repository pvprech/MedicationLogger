using MedikamentenLogger.Shared.Dtos.RatingDtos;

namespace MedikamentenLogger.Shared.Dtos.EntryDtos;

public record OpenedEntryDto(
    byte GeneralEffectiveness,
    byte GeneralSideEffects,
    string UserNote,
    List<EntryRatingDto> Ratings
);
