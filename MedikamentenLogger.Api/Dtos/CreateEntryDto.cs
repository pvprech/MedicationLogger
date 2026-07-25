using System.ComponentModel.DataAnnotations;

namespace MedikamentenLogger.Api.Dtos;

public record CreateEntryDto(
    int MedicationId,
    DateOnly Date,
    byte GenrealEffectiveness,
    byte GeneralSideEffects,
    [Required] string UserNote
);
