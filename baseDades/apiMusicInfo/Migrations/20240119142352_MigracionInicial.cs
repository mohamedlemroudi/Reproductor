using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiMusicInfo.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Band",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Band", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Extensions",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extensions", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Musician",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musician", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Dispositiu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlaylistName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => new { x.Dispositiu, x.PlaylistName });
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    VersionOriginalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OriginalSongUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Songs_Songs_OriginalSongUID",
                        column: x => x.OriginalSongUID,
                        principalTable: "Songs",
                        principalColumn: "UID");
                });

            migrationBuilder.CreateTable(
                name: "BandMusician",
                columns: table => new
                {
                    BandsName = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    MusiciansName = table.Column<string>(type: "nvarchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandMusician", x => new { x.BandsName, x.MusiciansName });
                    table.ForeignKey(
                        name: "FK_BandMusician_Band_BandsName",
                        column: x => x.BandsName,
                        principalTable: "Band",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandMusician_Musician_MusiciansName",
                        column: x => x.MusiciansName,
                        principalTable: "Musician",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    AlbumName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    SongUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => new { x.AlbumName, x.Year, x.SongUID });
                    table.ForeignKey(
                        name: "FK_Albums_Songs_SongUID",
                        column: x => x.SongUID,
                        principalTable: "Songs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionSong",
                columns: table => new
                {
                    ExtensionsName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SongsUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionSong", x => new { x.ExtensionsName, x.SongsUID });
                    table.ForeignKey(
                        name: "FK_ExtensionSong_Extensions_ExtensionsName",
                        column: x => x.ExtensionsName,
                        principalTable: "Extensions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtensionSong_Songs_SongsUID",
                        column: x => x.SongsUID,
                        principalTable: "Songs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSong",
                columns: table => new
                {
                    SongsUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaylistsDispositiu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlaylistsPlaylistName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSong", x => new { x.SongsUID, x.PlaylistsDispositiu, x.PlaylistsPlaylistName });
                    table.ForeignKey(
                        name: "FK_PlaylistSong_Playlists_PlaylistsDispositiu_PlaylistsPlaylistName",
                        columns: x => new { x.PlaylistsDispositiu, x.PlaylistsPlaylistName },
                        principalTable: "Playlists",
                        principalColumns: new[] { "Dispositiu", "PlaylistName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSong_Songs_SongsUID",
                        column: x => x.SongsUID,
                        principalTable: "Songs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plays",
                columns: table => new
                {
                    Bandname = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    MusicianName = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    InstrumentName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SongUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plays", x => new { x.SongUID, x.MusicianName, x.InstrumentName, x.Bandname });
                    table.ForeignKey(
                        name: "FK_Plays_Band_Bandname",
                        column: x => x.Bandname,
                        principalTable: "Band",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plays_Instruments_InstrumentName",
                        column: x => x.InstrumentName,
                        principalTable: "Instruments",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plays_Musician_MusicianName",
                        column: x => x.MusicianName,
                        principalTable: "Musician",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plays_Songs_SongUID",
                        column: x => x.SongUID,
                        principalTable: "Songs",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_SongUID",
                table: "Albums",
                column: "SongUID");

            migrationBuilder.CreateIndex(
                name: "IX_BandMusician_MusiciansName",
                table: "BandMusician",
                column: "MusiciansName");

            migrationBuilder.CreateIndex(
                name: "IX_Extensions_Name",
                table: "Extensions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionSong_SongsUID",
                table: "ExtensionSong",
                column: "SongsUID");

            migrationBuilder.CreateIndex(
                name: "IX_Musician_Name",
                table: "Musician",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_Dispositiu",
                table: "Playlists",
                column: "Dispositiu");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSong_PlaylistsDispositiu_PlaylistsPlaylistName",
                table: "PlaylistSong",
                columns: new[] { "PlaylistsDispositiu", "PlaylistsPlaylistName" });

            migrationBuilder.CreateIndex(
                name: "IX_Plays_Bandname",
                table: "Plays",
                column: "Bandname");

            migrationBuilder.CreateIndex(
                name: "IX_Plays_InstrumentName",
                table: "Plays",
                column: "InstrumentName");

            migrationBuilder.CreateIndex(
                name: "IX_Plays_MusicianName",
                table: "Plays",
                column: "MusicianName");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_OriginalSongUID",
                table: "Songs",
                column: "OriginalSongUID");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_UID",
                table: "Songs",
                column: "UID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "BandMusician");

            migrationBuilder.DropTable(
                name: "ExtensionSong");

            migrationBuilder.DropTable(
                name: "PlaylistSong");

            migrationBuilder.DropTable(
                name: "Plays");

            migrationBuilder.DropTable(
                name: "Extensions");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Band");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "Musician");

            migrationBuilder.DropTable(
                name: "Songs");
        }
    }
}
