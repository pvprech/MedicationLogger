namespace MedikamentenLogger.Api.Endpoints;

public static class StarRatingEndpoints
{
    public static void MapStarRatingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/starRatings");
    }
}
