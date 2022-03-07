using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YoutubeDownloader.Infrastructure.Migrations
{
    public partial class AddedDownloadHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AudioDownloadHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    VideoAuthor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    VideoTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VideoId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    BitrateInKilobytesPerSecond = table.Column<double>(type: "float", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioDownloadHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioDownloadHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoDownloadHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoAuthor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    VideoTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VideoId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    QualityLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BitrateInKilobytesPerSecond = table.Column<double>(type: "float", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoDownloadHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoDownloadHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioDownloadHistory_UserId",
                table: "AudioDownloadHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoDownloadHistory_UserId",
                table: "VideoDownloadHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioDownloadHistory");

            migrationBuilder.DropTable(
                name: "VideoDownloadHistory");
        }
    }
}
