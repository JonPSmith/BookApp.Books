using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModMon.Books.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WhenCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedDate = table.Column<bool>(type: "bit", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgPrice = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    ActualPrice = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    PromotionalText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageUrl = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ManningBookUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutAuthor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutReader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutTechnology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsInside = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorsOrdered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewsCount = table.Column<int>(type: "int", nullable: false),
                    ReviewsAverageVotes = table.Column<double>(type: "float", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    WhenCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthor",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<byte>(type: "tinyint", nullable: false),
                    WhenCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthor", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthor_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthor_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumStars = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    WhenCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookTag",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTag", x => new { x.BookId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BookTag_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_LastUpdatedUtc",
                table: "Authors",
                column: "LastUpdatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_AuthorId",
                table: "BookAuthor",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_LastUpdatedUtc",
                table: "BookAuthor",
                column: "LastUpdatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ActualPrice",
                table: "Books",
                column: "ActualPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LastUpdatedUtc",
                table: "Books",
                column: "LastUpdatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublishedOn",
                table: "Books",
                column: "PublishedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReviewsAverageVotes",
                table: "Books",
                column: "ReviewsAverageVotes");

            migrationBuilder.CreateIndex(
                name: "IX_Books_SoftDeleted",
                table: "Books",
                column: "SoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BookTag_TagId",
                table: "BookTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_BookId",
                table: "Review",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_LastUpdatedUtc",
                table: "Review",
                column: "LastUpdatedUtc");

            migrationBuilder.Sql(@"CREATE FUNCTION AuthorsStringUdf (@bookId int)
RETURNS NVARCHAR(4000)
AS
BEGIN
-- Thanks to https://stackoverflow.com/a/194887/1434764
DECLARE @Names AS NVARCHAR(4000)
SELECT @Names = COALESCE(@Names + ', ', '') + a.Name
FROM Authors AS a, Books AS b, BookAuthor AS ba 
WHERE ba.BookId = @bookId
      AND ba.AuthorId = a.AuthorId 
	  AND ba.BookId = b.BookId
ORDER BY ba.[Order]
RETURN @Names
END");

            migrationBuilder.Sql(@"CREATE FUNCTION TagsStringUdf (@bookId int)
RETURNS NVARCHAR(4000)
AS
BEGIN
-- Thanks to https://stackoverflow.com/a/194887/1434764
DECLARE @Tags AS NVARCHAR(4000)
SELECT @Tags = COALESCE(@Tags + ' | ', '') + t.TagId
FROM BookTag AS t, Books AS b 
WHERE t.BookId = @bookId AND b.BookId =  @bookId
RETURN @Tags
END
GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthor");

            migrationBuilder.DropTable(
                name: "BookTag");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.Sql(@"IF OBJECT_ID('dbo.AuthorsStringUdf') IS NOT NULL
	DROP FUNCTION dbo.AuthorsStringUdf");

            migrationBuilder.Sql(@"IF OBJECT_ID('dbo.TagsStringUdf') IS NOT NULL
	DROP FUNCTION dbo.TagsStringUdf");
        }
    }
}
