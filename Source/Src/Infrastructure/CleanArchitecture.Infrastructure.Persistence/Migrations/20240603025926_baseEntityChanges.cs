using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class baseEntityChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ApprovedCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedCards_TypeId",
                table: "ApprovedCards",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedCards_CardTypes_TypeId",
                table: "ApprovedCards",
                column: "TypeId",
                principalTable: "CardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedCards_CardTypes_TypeId",
                table: "ApprovedCards");

            migrationBuilder.DropIndex(
                name: "IX_ApprovedCards_TypeId",
                table: "ApprovedCards");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ApprovedCards");
        }
    }
}
