using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class History : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    MoveHistoryDTOId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Undo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Redo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrainDbDTOId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.MoveHistoryDTOId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History");
        }
    }
}
