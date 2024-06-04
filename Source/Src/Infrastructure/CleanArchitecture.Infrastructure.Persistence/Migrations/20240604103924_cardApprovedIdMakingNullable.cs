using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class cardApprovedIdMakingNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cards_ApprovedCardId",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedCardId",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ApprovedCardId",
                table: "Cards",
                column: "ApprovedCardId",
                unique: true,
                filter: "[ApprovedCardId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cards_ApprovedCardId",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedCardId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ApprovedCardId",
                table: "Cards",
                column: "ApprovedCardId",
                unique: true);
        }
    }
}
