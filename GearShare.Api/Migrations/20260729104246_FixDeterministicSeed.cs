using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearShare.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixDeterministicSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-0000-0000-0000-a1a1a1a1a1a1"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-0000-0000-0000-b2b2b2b2b2b2"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-0000-0000-0000-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-0000-0000-0000-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-0000-0000-0000-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-0000-0000-0000-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "$2a$11$9OoRVbfMGZ3WlM0Xp1bMCOqhqVSsDL7GO6OfpTJo9VsB8zfCGCeRa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "$2a$11$9OoRVbfMGZ3WlM0Xp1bMCOqhqVSsDL7GO6OfpTJo9VsB8zfCGCeRa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-0000-0000-0000-a1a1a1a1a1a1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 19, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(6866));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(5644));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-0000-0000-0000-b2b2b2b2b2b2"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 24, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(6871));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 4, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(6828));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-0000-0000-0000-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(6849));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-0000-0000-0000-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 11, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(6853));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-0000-0000-0000-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(6857));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-0000-0000-0000-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 17, 10, 32, 46, 386, DateTimeKind.Utc).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "$2a$11$sNQ53T8TAMf5w57CKqXnausFpxHt8rqI0e6NfQddJLwg1ZkXvrccu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "$2a$11$jHXXcPwVvM3jRSenntQKA.Aeh4IRpAp15qpnk/q0fssMd/KrbRgQO");
        }
    }
}
