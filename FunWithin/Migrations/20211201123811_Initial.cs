using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FunWithin.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: true),
                    ReviewAuthor = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Genre = table.Column<string>(type: "text", nullable: true),
                    ReviewText = table.Column<string[]>(type: "text[]", nullable: true),
                    PublicationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    ReviewLikesUsersId = table.Column<int[]>(type: "integer[]", nullable: true),
                    AverageItemRating = table.Column<decimal>(type: "numeric", nullable: false),
                    ItemGrade = table.Column<int[]>(type: "integer[]", nullable: true),
                    GradeUserId = table.Column<int[]>(type: "integer[]", nullable: true),
                    Cover = table.Column<string[]>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
