namespace MedikamentenLogger.Api.Dtos;

public record OpenedEntryDto(
    byte GeneralEffectiveness,
    byte GeneralSideEffects,
    string UserNote,
    List<EntryRatingDto> Ratings
);
