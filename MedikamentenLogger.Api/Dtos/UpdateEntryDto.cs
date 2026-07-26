using System.ComponentModel.DataAnnotations;

namespace MedikamentenLogger.Api.Dtos;

public record UpdateEntryDto(
    [Required][Range(0, 10)] byte GeneralEffectiveness,
    [Required][Range(0, 10)] byte GeneralSideEffects,
    [Required] string UserNote,
    [Required] List<UpdateEntryRatingDto> Ratings
);