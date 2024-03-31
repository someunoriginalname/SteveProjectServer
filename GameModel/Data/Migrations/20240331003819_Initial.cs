using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherID = table.Column<int>(type: "int", nullable: false),
                    PublisherName = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.PublisherID);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    AppID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Players = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Developer = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    GameName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PublisherID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.AppID);
                    table.ForeignKey(
                        name: "FK_Game_Publisher",
                        column: x => x.PublisherID,
                        principalTable: "Publishers",
                        principalColumn: "PublisherID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_PublisherID",
                table: "Game",
                column: "PublisherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Publishers");
        }
    }
}
