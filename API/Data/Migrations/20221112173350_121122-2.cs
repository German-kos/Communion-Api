using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class _1211222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Threads_ThreadId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_AuthorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Categories_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_SubCategories_SubCategoryId",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Users_AuthorId",
                table: "Threads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Threads",
                table: "Threads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Threads",
                newName: "ForumThread");

            migrationBuilder.RenameTable(
                name: "SubCategories",
                newName: "ForumSubCategory");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "ForumComment");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_SubCategoryId",
                table: "ForumThread",
                newName: "IX_ForumThread_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_AuthorId",
                table: "ForumThread",
                newName: "IX_ForumThread_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_CategoryId",
                table: "ForumSubCategory",
                newName: "IX_ForumSubCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ThreadId",
                table: "ForumComment",
                newName: "IX_ForumComment_ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AuthorId",
                table: "ForumComment",
                newName: "IX_ForumComment_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumThread",
                table: "ForumThread",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumSubCategory",
                table: "ForumSubCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumComment",
                table: "ForumComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_ForumThread_ThreadId",
                table: "ForumComment",
                column: "ThreadId",
                principalTable: "ForumThread",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumComment_Users_AuthorId",
                table: "ForumComment",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumSubCategory_Categories_CategoryId",
                table: "ForumSubCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_ForumSubCategory_SubCategoryId",
                table: "ForumThread",
                column: "SubCategoryId",
                principalTable: "ForumSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_Users_AuthorId",
                table: "ForumThread",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_ForumThread_ThreadId",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumComment_Users_AuthorId",
                table: "ForumComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumSubCategory_Categories_CategoryId",
                table: "ForumSubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_ForumSubCategory_SubCategoryId",
                table: "ForumThread");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_Users_AuthorId",
                table: "ForumThread");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumThread",
                table: "ForumThread");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumSubCategory",
                table: "ForumSubCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumComment",
                table: "ForumComment");

            migrationBuilder.RenameTable(
                name: "ForumThread",
                newName: "Threads");

            migrationBuilder.RenameTable(
                name: "ForumSubCategory",
                newName: "SubCategories");

            migrationBuilder.RenameTable(
                name: "ForumComment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_ForumThread_SubCategoryId",
                table: "Threads",
                newName: "IX_Threads_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumThread_AuthorId",
                table: "Threads",
                newName: "IX_Threads_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumSubCategory_CategoryId",
                table: "SubCategories",
                newName: "IX_SubCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_ThreadId",
                table: "Comments",
                newName: "IX_Comments_ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumComment_AuthorId",
                table: "Comments",
                newName: "IX_Comments_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Threads",
                table: "Threads",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Threads_ThreadId",
                table: "Comments",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Categories_CategoryId",
                table: "SubCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_SubCategories_SubCategoryId",
                table: "Threads",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Users_AuthorId",
                table: "Threads",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
