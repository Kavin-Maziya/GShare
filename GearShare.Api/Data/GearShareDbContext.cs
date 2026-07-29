using GearShare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GearShare.Api.Data;

public class GearShareDbContext(DbContextOptions<GearShareDbContext> options)
    : DbContext(options)
{
    public DbSet<User>          Users          => Set<User>();
    public DbSet<GearItem>      GearItems      => Set<GearItem>();
    public DbSet<RentalRequest> RentalRequests => Set<RentalRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── User ────────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Email).IsRequired().HasMaxLength(256);
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.Name).IsRequired().HasMaxLength(100);
            e.Property(u => u.Role)
             .HasConversion<string>()
             .HasMaxLength(20);
            e.HasIndex(u => u.Email).IsUnique();
        });

        // ── GearItem ────────────────────────────────────────────────────────
        modelBuilder.Entity<GearItem>(e =>
        {
            e.HasKey(g => g.Id);
            e.Property(g => g.Title).IsRequired().HasMaxLength(200);
            e.Property(g => g.Description).IsRequired().HasMaxLength(2000);
            e.Property(g => g.Category).HasConversion<string>().HasMaxLength(20);
            e.Property(g => g.Status).HasConversion<string>().HasMaxLength(20);
            e.Property(g => g.DailyRateCents).IsRequired();
            e.Property(g => g.CreatedAt).IsRequired();

            e.Property(g => g.SearchVector)
             .HasColumnName("search_vector")
             .HasComputedColumnSql("to_tsvector('english', \"Title\" || ' ' || \"Description\")", stored: true);

            e.HasOne(g => g.Owner)
             .WithMany(u => u.GearItems)
             .HasForeignKey(g => g.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── RentalRequest ────────────────────────────────────────────────────
        modelBuilder.Entity<RentalRequest>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.RenterName).IsRequired().HasMaxLength(100);
            e.Property(r => r.RenterEmail).IsRequired().HasMaxLength(256);
            e.Property(r => r.RenterPhone).IsRequired().HasMaxLength(20);
            e.Property(r => r.Notes).HasMaxLength(1000);
            e.Property(r => r.RequestedAt).IsRequired();
            e.Property(r => r.Status).HasConversion<string>().HasMaxLength(20);

            e.HasOne(r => r.GearItem)
             .WithMany(g => g.RentalRequests)
             .HasForeignKey(r => r.GearItemId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Seed data ────────────────────────────────────────────────────────
        // All values are hardcoded static literals — no DateTime.UtcNow, no
        // Guid.NewGuid(), no BCrypt.HashPassword() — those recompute on every
        // build and cause PendingModelChangesWarning.
        // Password hashes below are pre-computed bcrypt hashes of "password".
        var ownerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var adminId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id           = ownerId,
                Email        = "alice@example.com",
                PasswordHash = "$2a$11$9OoRVbfMGZ3WlM0Xp1bMCOqhqVSsDL7GO6OfpTJo9VsB8zfCGCeRa",
                Name         = "Alice",
                Role         = UserRole.Member
            },
            new User
            {
                Id           = adminId,
                Email        = "admin@example.com",
                PasswordHash = "$2a$11$9OoRVbfMGZ3WlM0Xp1bMCOqhqVSsDL7GO6OfpTJo9VsB8zfCGCeRa",
                Name         = "Admin User",
                Role         = UserRole.Admin
            }
        );

        modelBuilder.Entity<GearItem>().HasData(
            new GearItem { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), OwnerId = ownerId, Title = "4-Person Camping Tent",  Description = "Spacious tent for family trips.",         Category = Category.Camping,  DailyRateCents = 2500, Status = GearStatus.Available, CreatedAt = new DateTime(2026, 5, 1,  0, 0, 0, DateTimeKind.Utc) },
            new GearItem { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), OwnerId = ownerId, Title = "Kayak - Single",          Description = "Lightweight kayak for calm water.",       Category = Category.Water,    DailyRateCents = 4500, Status = GearStatus.Available, CreatedAt = new DateTime(2026, 5, 5,  0, 0, 0, DateTimeKind.Utc) },
            new GearItem { Id = Guid.Parse("cccccccc-0000-0000-0000-cccccccccccc"), OwnerId = ownerId, Title = "Climbing Harness",        Description = "Beginner-friendly climbing harness.",     Category = Category.Climbing, DailyRateCents = 1500, Status = GearStatus.Available, CreatedAt = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc) },
            new GearItem { Id = Guid.Parse("dddddddd-0000-0000-0000-dddddddddddd"), OwnerId = ownerId, Title = "Ski Poles - Pair",        Description = "Adjustable poles for all skill levels.", Category = Category.Winter,   DailyRateCents = 800,  Status = GearStatus.Available, CreatedAt = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc) },
            new GearItem { Id = Guid.Parse("eeeeeeee-0000-0000-0000-eeeeeeeeeeee"), OwnerId = ownerId, Title = "Mountain Bike",           Description = "Full-suspension trail bike.",             Category = Category.Cycling,  DailyRateCents = 6000, Status = GearStatus.Available, CreatedAt = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc) },
            new GearItem { Id = Guid.Parse("ffffffff-0000-0000-0000-ffffffffffff"), OwnerId = ownerId, Title = "Wetsuit - Medium",        Description = "5mm wetsuit for cold water.",             Category = Category.Water,    DailyRateCents = 3000, Status = GearStatus.Available, CreatedAt = new DateTime(2026, 5, 25, 0, 0, 0, DateTimeKind.Utc) },
            new GearItem { Id = Guid.Parse("a1a1a1a1-0000-0000-0000-a1a1a1a1a1a1"), OwnerId = ownerId, Title = "Snowboard + Bindings",   Description = "All-mountain board, size 156.",           Category = Category.Winter,   DailyRateCents = 5500, Status = GearStatus.Available, CreatedAt = new DateTime(2026, 6, 1,  0, 0, 0, DateTimeKind.Utc) },
            new GearItem { Id = Guid.Parse("b2b2b2b2-0000-0000-0000-b2b2b2b2b2b2"), OwnerId = ownerId, Title = "Rope - 60m Dry",        Description = "Dry-treated climbing rope.",              Category = Category.Climbing, DailyRateCents = 2000, Status = GearStatus.Available, CreatedAt = new DateTime(2026, 6, 5,  0, 0, 0, DateTimeKind.Utc) }
        );
    }
}