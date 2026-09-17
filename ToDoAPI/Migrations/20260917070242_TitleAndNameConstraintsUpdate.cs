using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAPI.Migrations
{
    /// <inheritdoc />
    public partial class TitleAndNameConstraintsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Tasks_Title",
                table: "Tasks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Categories_Name",
                table: "Categories");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tasks_Title",
                table: "Tasks",
                sql: "LEN(TRIM([Title])) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Categories_Name",
                table: "Categories",
                sql: "LEN(TRIM([Name])) > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Tasks_Title",
                table: "Tasks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Categories_Name",
                table: "Categories");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tasks_Title",
                table: "Tasks",
                sql: "LEN(TRIM([Title])) > 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Categories_Name",
                table: "Categories",
                sql: "LEN(TRIM([Name])) > 1");
        }
    }
}
