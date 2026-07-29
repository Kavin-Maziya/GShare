using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace GearShare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add generated tsvector column combining Title and Description.
            // Must use raw SQL — EF's AddColumn cannot express GENERATED ALWAYS AS for tsvector.
            // 'english' stemming means plurals and tense variants match the same lexeme.
            migrationBuilder.Sql(@"
        ALTER TABLE ""GearItems""
        ADD COLUMN search_vector tsvector
        GENERATED ALWAYS AS (
            to_tsvector('english', ""Title"" || ' ' || ""Description"")
        ) STORED;
    ");

            // GIN index — without this, full-text search falls back to a sequential scan.
            // EXPLAIN ANALYZE in README confirms the index is used after this migration.
            migrationBuilder.Sql(@"
        CREATE INDEX ix_gearitems_search_vector
        ON ""GearItems"" USING GIN (search_vector);
    ");

            // UpdateData calls below are kept — EF generated them for the seed timestamps
            // and password hashes; they are safe to leave in place.
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_gearitems_search_vector;");
            migrationBuilder.Sql(@"ALTER TABLE ""GearItems"" DROP COLUMN IF EXISTS search_vector;");

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-0000-0000-0000-a1a1a1a1a1a1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 19, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5656));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(3709));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-0000-0000-0000-b2b2b2b2b2b2"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 24, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5663));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 4, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5601));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-0000-0000-0000-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5630));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-0000-0000-0000-dddddddddddd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 11, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5637));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-0000-0000-0000-eeeeeeeeeeee"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 14, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5643));

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-0000-0000-0000-ffffffffffff"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 17, 10, 28, 3, 234, DateTimeKind.Utc).AddTicks(5650));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "$2a$11$qVoZScKJ/tbDjNdOeL3sIueh/iXK8L51siIESiFH//4Vb/8XIxobS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "$2a$11$CrS8co4U7EA9ldadkveLZ.dFxhA9.Lk02c1uCdSXvisMTjZtV5uGm");
        }
    }
}
