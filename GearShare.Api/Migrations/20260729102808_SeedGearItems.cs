using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GearShare.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedGearItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "alice@example.com", "Alice", "$2a$11$qVoZScKJ/tbDjNdOeL3sIueh/iXK8L51siIESiFH//4Vb/8XIxobS", "Member" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "admin@example.com", "Admin User", "$2a$11$CrS8co4U7EA9ldadkveLZ.dFxhA9.Lk02c1uCdSXvisMTjZtV5uGm", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "GearItems",
                columns: new[] { "Id", "Category", "CreatedAt", "DailyRateCents", "Description", "OwnerId", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-0000-0000-0000-a1a1a1a1a1a1"), "Winter", new DateTime(2026, 7, 19, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5656), 5500, "All-mountain board, size 156.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "Snowboard + Bindings" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Camping", new DateTime(2026, 6, 29, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(3709), 2500, "Spacious tent for family trips.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "4-Person Camping Tent" },
                    { new Guid("b2b2b2b2-0000-0000-0000-b2b2b2b2b2b2"), "Climbing", new DateTime(2026, 7, 24, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5663), 2000, "Dry-treated climbing rope.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "Rope - 60m Dry" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Water", new DateTime(2026, 7, 4, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5601), 4500, "Lightweight kayak for calm water.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "Kayak - Single" },
                    { new Guid("cccccccc-0000-0000-0000-cccccccccccc"), "Climbing", new DateTime(2026, 7, 9, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5630), 1500, "Beginner-friendly climbing harness.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "Climbing Harness" },
                    { new Guid("dddddddd-0000-0000-0000-dddddddddddd"), "Winter", new DateTime(2026, 7, 11, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5637), 800, "Adjustable poles for all skill levels.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "Ski Poles - Pair" },
                    { new Guid("eeeeeeee-0000-0000-0000-eeeeeeeeeeee"), "Cycling", new DateTime(2026, 7, 14, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5643), 6000, "Full-suspension trail bike.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "Mountain Bike" },
                    { new Guid("ffffffff-0000-0000-0000-ffffffffffff"), "Water", new DateTime(2026, 7, 17, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5650), 3000, "5mm wetsuit for cold water.", new Guid("11111111-1111-1111-1111-111111111111"), "Available", "Wetsuit - Medium" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-0000-0000-0000-a1a1a1a1a1a1"));

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-0000-0000-0000-b2b2b2b2b2b2"));

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-0000-0000-0000-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-0000-0000-0000-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-0000-0000-0000-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-0000-0000-0000-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
