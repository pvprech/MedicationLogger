using MedikamentenLogger.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedikamentenLogger.Api.Data;

public class MLContext(DbContextOptions<MLContext> options)
    : DbContext(options)
{
    public DbSet<Entry> Entries => Set<Entry>();
    public DbSet<EntryRating> EntryRatings => Set<EntryRating>();
    public DbSet<StarRating> StarRatings => Set<StarRating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EntryRating>()
            .HasKey(r => new { r.StarRatingId, r.EntryId });

        modelBuilder.Entity<EntryRating>()
            .HasOne<Entry>()
            .WithMany()
            .HasForeignKey(r => r.EntryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EntryRating>()
            .HasOne<StarRating>()
            .WithMany()
            .HasForeignKey(r => r.StarRatingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
