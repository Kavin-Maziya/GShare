namespace GearShare.Api.Models;

public class GearItem
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Category Category { get; set; }
    public int DailyRateCents { get; set; }
    public GearStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
    // Generated tsvector column — populated by PostgreSQL, never set by the app.
    // Mapped as a computed column so EF reads it but never writes to it.
    public NpgsqlTypes.NpgsqlTsVector SearchVector { get; set; } = null!;
}