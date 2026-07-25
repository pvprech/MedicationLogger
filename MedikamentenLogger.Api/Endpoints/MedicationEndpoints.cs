namespace MedikamentenLogger.Api.Endpoints;

public static class MedicationEndpoints
{
    public static void MapMedicationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/medications");
    }
}
