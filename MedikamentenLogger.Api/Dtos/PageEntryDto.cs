namespace MedikamentenLogger.Api.Dtos;

public record PageEntryDto(
    int Id,
    int MedicationId,
    DateOnly Date
);
