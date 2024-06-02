using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class townCardUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TownCardTypeMasterDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ApplicationTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    PriorityOrder = table.Column<byte>(type: "tinyint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TownCardTypeMasterDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMapAddressUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDateToShow = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GoogleProfileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaceBookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherReferenceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TownApprovedCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    TownId = table.Column<int>(type: "int", nullable: false),
                    ApprovedCount = table.Column<int>(type: "int", nullable: false),
                    RejectedCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DisLikeCount = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMapAddressUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDateToShow = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GoogleProfileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaceBookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherReferenceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TownApprovedCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TownApprovedCards_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectedDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ApprovedCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedDate_TownApprovedCards_ApprovedCardId",
                        column: x => x.ApprovedCardId,
                        principalTable: "TownApprovedCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TownCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovedCardId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    TownId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleMapAddressUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDateToShow = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GoogleProfileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaceBookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherReferenceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TownCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TownCards_TownApprovedCards_ApprovedCardId",
                        column: x => x.ApprovedCardId,
                        principalTable: "TownApprovedCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TownCards_TownCardTypeMasterDatas_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TownCardTypeMasterDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TownCards_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TownCardApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TownCardId = table.Column<int>(type: "int", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TownCardApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TownCardApprovals_TownCards_TownCardId",
                        column: x => x.TownCardId,
                        principalTable: "TownCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectedDate_ApprovedCardId",
                table: "SelectedDate",
                column: "ApprovedCardId");

            migrationBuilder.CreateIndex(
                name: "IX_TownApprovedCards_TownId",
                table: "TownApprovedCards",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_TownCardApprovals_TownCardId",
                table: "TownCardApprovals",
                column: "TownCardId");

            migrationBuilder.CreateIndex(
                name: "IX_TownCards_ApprovedCardId",
                table: "TownCards",
                column: "ApprovedCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TownCards_TownId",
                table: "TownCards",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_TownCards_TypeId",
                table: "TownCards",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SelectedDate");

            migrationBuilder.DropTable(
                name: "TownCardApprovals");

            migrationBuilder.DropTable(
                name: "TownCards");

            migrationBuilder.DropTable(
                name: "TownApprovedCards");

            migrationBuilder.DropTable(
                name: "TownCardTypeMasterDatas");

            migrationBuilder.DropTable(
                name: "Towns");
        }
    }
}
