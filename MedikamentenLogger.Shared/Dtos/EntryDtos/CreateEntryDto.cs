using System.ComponentModel.DataAnnotations;

namespace MedikamentenLogger.Shared.Dtos.EntryDtos;

public record CreateEntryDto(
    [Required] int MedicationId,
    [Required] DateOnly Date,
    [Required][Range(0, 10)] byte GenrealEffectiveness,
    [Required][Range(0, 10)] byte GeneralSideEffects,
    [Required] string UserNote
);

