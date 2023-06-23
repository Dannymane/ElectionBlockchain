using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectionBlockchain.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaderSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstVerifierSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondVerifierSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousBlockId = table.Column<int>(type: "int", nullable: true),
                    PreviousBlockHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocks_Blocks_PreviousBlockId",
                        column: x => x.PreviousBlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VotesQueue",
                columns: table => new
                {
                    CitizenDocumentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CitizenSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotesQueue", x => x.CitizenDocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    CitizenDocumentId = table.Column<string>(type: "nvarchar(6)", nullable: false),
                    CitizenSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    BlockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.CitizenDocumentId);
                    table.ForeignKey(
                        name: "FK_Votes_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votes_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Citizens_CitizenDocumentId",
                        column: x => x.CitizenDocumentId,
                        principalTable: "Citizens",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_PreviousBlockId",
                table: "Blocks",
                column: "PreviousBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_BlockId",
                table: "Votes",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_CandidateId",
                table: "Votes",
                column: "CandidateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "VotesQueue");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Citizens");
        }
    }
}
