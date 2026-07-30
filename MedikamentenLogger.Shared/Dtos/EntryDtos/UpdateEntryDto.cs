using System.ComponentModel.DataAnnotations;
using MedikamentenLogger.Shared.Dtos.RatingDtos;

namespace MedikamentenLogger.Shared.Dtos.EntryDtos;

public record UpdateEntryDto(
    [Required][Range(0, 10)] byte GeneralEffectiveness,
    [Required][Range(0, 10)] byte GeneralSideEffects,
    [Required] string UserNote,
    [Required] List<UpdateEntryRatingDto> Ratings
);