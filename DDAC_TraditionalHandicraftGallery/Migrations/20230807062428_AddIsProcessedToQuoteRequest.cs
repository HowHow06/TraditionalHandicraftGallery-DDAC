using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDAC_TraditionalHandicraftGallery.Migrations
{
    /// <inheritdoc />
    public partial class AddIsProcessedToQuoteRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "QuoteRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "QuoteRequests");
        }
    }
}
