using System.ComponentModel.DataAnnotations;

namespace MedikamentenLogger.Api.Dtos;

public record UpdateEntryDto(
    DateOnly Date,
    byte GeneralEffectiveness,
    byte GeneralSideEffects,
    [Required] string UserNote
);