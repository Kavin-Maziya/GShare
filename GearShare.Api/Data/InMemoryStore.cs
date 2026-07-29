using GearShare.Api.Models;

namespace GearShare.Api.Data;

public static class InMemoryStore
{
    public static List<User> Users { get; } = new()
    {
        new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Email = "alice@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Name = "Alice",
            Role = UserRole.Member
        },
        new User
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Name = "Admin User",
            Role = UserRole.Admin
        }
    };

    public static List<GearItem> GearItems { get; } = new()
    {
        new GearItem
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Title = "4-Person Camping Tent",
            Description = "Spacious tent perfect for family camping trips.",
            Category = Category.Camping,
            DailyRateCents = 2500,
            Status = GearStatus.Available,
            CreatedAt = DateTime.UtcNow.AddDays(-30)
        },
        new GearItem
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Title = "Kayak - Single",
            Description = "Lightweight kayak for calm water paddling.",
            Category = Category.Water,
            DailyRateCents = 4500,
            Status = GearStatus.Available,
            CreatedAt = DateTime.UtcNow.AddDays(-15)
        }
    };

    public static List<RentalRequest> RentalRequests { get; } = new();
}