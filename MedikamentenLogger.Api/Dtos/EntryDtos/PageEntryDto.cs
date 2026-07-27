namespace MedikamentenLogger.Api.Dtos.EntryDtos;

public record PageEntryDto(
    int Id,
    int MedicationId,
    DateOnly Date
);
