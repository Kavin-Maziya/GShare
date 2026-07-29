using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GearShare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOverlapConstraint : Migration
    {
       protected override void Up(MigrationBuilder migrationBuilder)
{
    // btree_gist enables GiST indexing for scalar types (like uuid and daterange together).
    // Without it, PostgreSQL cannot build a GiST index on a mix of btree-comparable columns.
    migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS btree_gist;");

    // Add a generated (stored) daterange column derived from StartDate and EndDate.
    // '[]' means inclusive on both ends — the full rental period is booked.
    migrationBuilder.Sql(@"
        ALTER TABLE ""RentalRequests""
        ADD COLUMN date_range daterange
        GENERATED ALWAYS AS (daterange(""StartDate"", ""EndDate"", '[]')) STORED;
    ");

    // EXCLUDE constraint: for any two rows with the same GearItemId and Status = 'Approved',
    // their date_range columns must NOT overlap (&&).
    // This is enforced at the database level — application code alone cannot prevent
    // two concurrent transactions both passing an app-level overlap check before committing.
    migrationBuilder.Sql(@"
        ALTER TABLE ""RentalRequests""
        ADD CONSTRAINT no_overlapping_approved_rentals
        EXCLUDE USING gist (
            ""GearItemId"" WITH =,
            date_range WITH &&
        ) WHERE (""Status"" = 'Approved');
    ");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"
        ALTER TABLE ""RentalRequests""
        DROP CONSTRAINT IF EXISTS no_overlapping_approved_rentals;
    ");

    migrationBuilder.Sql(@"
        ALTER TABLE ""RentalRequests""
        DROP COLUMN IF EXISTS date_range;
    ");

    migrationBuilder.Sql("DROP EXTENSION IF EXISTS btree_gist;");
}
    }
}
