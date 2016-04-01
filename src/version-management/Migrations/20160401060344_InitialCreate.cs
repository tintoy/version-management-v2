using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace versionmanagement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "VersionRange",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EndVersionBuild = table.Column<int>(nullable: false),
                    EndVersionMajor = table.Column<int>(nullable: false),
                    EndVersionMinor = table.Column<int>(nullable: false),
                    EndVersionRevision = table.Column<int>(nullable: false),
                    IncrementBy = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NextVersionBuild = table.Column<int>(nullable: false),
                    NextVersionMajor = table.Column<int>(nullable: false),
                    NextVersionMinor = table.Column<int>(nullable: false),
                    NextVersionRevision = table.Column<int>(nullable: false),
                    StartVersionBuild = table.Column<int>(nullable: false),
                    StartVersionMajor = table.Column<int>(nullable: false),
                    StartVersionMinor = table.Column<int>(nullable: false),
                    StartVersionRevision = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionRange", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Release",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    SpecialVersion = table.Column<string>(nullable: true),
                    VersionRangeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Release", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Release_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Release_VersionRange_VersionRangeId",
                        column: x => x.VersionRangeId,
                        principalTable: "VersionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "ReleaseVersion",
                columns: table => new
                {
                    CommitId = table.Column<string>(nullable: false),
                    ReleaseId = table.Column<int>(nullable: false),
                    SpecialVersion = table.Column<string>(nullable: false),
                    VersionBuild = table.Column<int>(nullable: false),
                    VersionMajor = table.Column<int>(nullable: false),
                    VersionMinor = table.Column<int>(nullable: false),
                    VersionRevision = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseVersion", x => new { x.CommitId, x.ReleaseId });
                    table.ForeignKey(
                        name: "FK_ReleaseVersion_Release_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Release",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ReleaseVersion");
            migrationBuilder.DropTable("Release");
            migrationBuilder.DropTable("Product");
            migrationBuilder.DropTable("VersionRange");
        }
    }
}
