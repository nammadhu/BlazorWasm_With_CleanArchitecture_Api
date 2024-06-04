using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class cardApprovedIdMakingNullable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                table: "SelectedDates");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "SelectedDates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "SelectedDates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "SelectedDates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "SelectedDates",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "SelectedDates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SelectedDates");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "SelectedDates");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "SelectedDates");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "SelectedDates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
