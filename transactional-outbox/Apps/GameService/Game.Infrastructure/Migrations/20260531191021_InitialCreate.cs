using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stadiums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HomeTeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stadiums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StadiumSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StadiumId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StadiumSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StadiumSeats_Stadiums_StadiumId",
                        column: x => x.StadiumId,
                        principalTable: "Stadiums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StadiumId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Stadiums_StadiumId",
                        column: x => x.StadiumId,
                        principalTable: "Stadiums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HomeTeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    AwayTeamId = table.Column<Guid>(type: "uuid", nullable: false),
                    StadiumId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Stadiums_StadiumId",
                        column: x => x.StadiumId,
                        principalTable: "Stadiums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameTickets_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTickets_StadiumSeats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "StadiumSeats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_AwayTeamId",
                table: "Games",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_HomeTeamId",
                table: "Games",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_StadiumId",
                table: "Games",
                column: "StadiumId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTickets_GameId",
                table: "GameTickets",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTickets_SeatId",
                table: "GameTickets",
                column: "SeatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSeats_StadiumId",
                table: "StadiumSeats",
                column: "StadiumId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_StadiumId",
                table: "Teams",
                column: "StadiumId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTickets");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "StadiumSeats");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Stadiums");
        }
    }
}
