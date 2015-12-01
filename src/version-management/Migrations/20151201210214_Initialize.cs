using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DD.Cloud.VersionManagement.Migrations
{
    public partial class Initialize : Migration
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
	        migrationBuilder.CreateIndex("IX_Product_Name", table: "Product", column: "Name");

            migrationBuilder.CreateTable(
                name: "Release",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
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
                });
			migrationBuilder.CreateIndex("IX_Release_Name", table: "Release", column: "Name");
			migrationBuilder.CreateIndex("IX_Release_ProductId_Name", table: "Product", columns: new [] { "ProductId", "Name" });
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.DropIndex("IX_Product_Name");
            migrationBuilder.DropTable("Release");

			migrationBuilder.DropIndex("IX_Release_Name");
			migrationBuilder.DropIndex("IX_Release_ProductId_Name");
			migrationBuilder.DropTable("Product");
        }
    }
}
