using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class _121122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Categories_CategoryId",
                table: "Banners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banners",
                table: "Banners");

            migrationBuilder.DropIndex(
                name: "IX_Banners_CategoryId",
                table: "Banners");

            migrationBuilder.RenameTable(
                name: "Banners",
                newName: "ForumImage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Threads",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<bool>(
                name: "Edited",
                table: "Threads",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeEdited",
                table: "Threads",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeEdited",
                table: "Comments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumImage",
                table: "ForumImage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ForumImage_CategoryId",
                table: "ForumImage",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumImage_Categories_CategoryId",
                table: "ForumImage",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumImage_Categories_CategoryId",
                table: "ForumImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumImage",
                table: "ForumImage");

            migrationBuilder.DropIndex(
                name: "IX_ForumImage_CategoryId",
                table: "ForumImage");

            migrationBuilder.DropColumn(
                name: "Edited",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "TimeEdited",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "TimeEdited",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "ForumImage",
                newName: "Banners");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Threads",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banners",
                table: "Banners",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_CategoryId",
                table: "Banners",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Categories_CategoryId",
                table: "Banners",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
