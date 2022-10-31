using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class _ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Categories_CategoryId",
                table: "Threads");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Threads",
                newName: "SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_CategoryId",
                table: "Threads",
                newName: "IX_Threads_SubCategoryId");

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_SubCategories_SubCategoryId",
                table: "Threads",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_SubCategories_SubCategoryId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "Threads",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_SubCategoryId",
                table: "Threads",
                newName: "IX_Threads_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Categories_CategoryId",
                table: "Threads",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
