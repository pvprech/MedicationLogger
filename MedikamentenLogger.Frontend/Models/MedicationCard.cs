namespace MedikamentenLogger.Frontend.Models;

public class MedicationCard
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImagePath { get; set; }
}