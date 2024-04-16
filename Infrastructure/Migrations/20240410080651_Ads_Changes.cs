using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Ads_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Ads\" ALTER COLUMN \"Images\" TYPE text[] USING ARRAY[\"Images\"];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "Ads",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");
        }
    }
}
