using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tests.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Nullable = table.Column<string>(type: "TEXT", nullable: true),
                    UniqueName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestModels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TestModels",
                columns: new[] { "Id", "Name", "Nullable", "UniqueName" },
                values: new object[] { 1, "TestModel1", "", "TestModel1" });

            migrationBuilder.InsertData(
                table: "TestModels",
                columns: new[] { "Id", "Name", "Nullable", "UniqueName" },
                values: new object[] { 2, "TestModel2", "", "TestModel2" });

            migrationBuilder.InsertData(
                table: "TestModels",
                columns: new[] { "Id", "Name", "Nullable", "UniqueName" },
                values: new object[] { 3, "TestModel3", "", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestModels");
        }
    }
}
