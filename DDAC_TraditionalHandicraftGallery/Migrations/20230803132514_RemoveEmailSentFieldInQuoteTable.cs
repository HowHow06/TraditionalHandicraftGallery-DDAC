using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDAC_TraditionalHandicraftGallery.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailSentFieldInQuoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSent",
                table: "QuoteRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailSent",
                table: "QuoteRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
