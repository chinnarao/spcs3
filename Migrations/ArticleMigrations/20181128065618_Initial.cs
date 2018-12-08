using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.ArticleMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(unicode: false, nullable: false),
                    Content = table.Column<string>(nullable: false),
                    UserIdOrEmail = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    UserLoggedInSocialProviderName = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    UserPhoneNumber = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    UserSocialAvatarUrl = table.Column<string>(unicode: false, nullable: true),
                    BiodataUrl = table.Column<string>(unicode: false, nullable: true),
                    HireMe = table.Column<bool>(nullable: false, defaultValue: false),
                    OpenSourceUrls = table.Column<string>(unicode: false, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: false),
                    IsArticleInDraftMode = table.Column<bool>(nullable: false, defaultValue: false),
                    IsPublished = table.Column<bool>(nullable: false, defaultValue: false),
                    AttachedAssetsInCloudCount = table.Column<int>(nullable: false),
                    AttachedAssetsInCloudStorageId = table.Column<Guid>(nullable: false),
                    AttachedAssetsStoredInCloudBaseFolderPath = table.Column<string>(unicode: false, nullable: true),
                    AllRelatedSubjectsIncludesVersionsWithComma = table.Column<string>(unicode: false, maxLength: 1000, nullable: true),
                    AttachmentURLsComma = table.Column<string>(unicode: false, nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    SocialURLsWithComma = table.Column<string>(unicode: false, nullable: true),
                    TotalVotes = table.Column<int>(nullable: true),
                    TotalVotedPersonsCount = table.Column<int>(nullable: true),
                    ArticleAverageVotes = table.Column<double>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    Tag1 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag2 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag3 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag4 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag5 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag6 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag7 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag8 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag9 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag10 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag11 = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    Tag12 = table.Column<string>(unicode: false, maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "ArticleComment",
                columns: table => new
                {
                    ArticleCommentId = table.Column<long>(nullable: false),
                    ArticleId = table.Column<long>(nullable: false),
                    UserIdOrEmail = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    UserSocialAvatarUrl = table.Column<string>(unicode: false, nullable: true),
                    IsAdminCommented = table.Column<bool>(nullable: true),
                    CommentedDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    Comment = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComment", x => x.ArticleCommentId);
                    table.ForeignKey(
                        name: "FK_ArticleComment_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleCommit",
                columns: table => new
                {
                    ArticleCommitId = table.Column<long>(nullable: false),
                    ArticleId = table.Column<long>(nullable: false),
                    CommittedDate = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    UserIdOrEmail = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    UserSocialAvatarUrl = table.Column<string>(unicode: false, nullable: true),
                    IsAdminCommited = table.Column<bool>(nullable: true),
                    Commit = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCommit", x => x.ArticleCommitId);
                    table.ForeignKey(
                        name: "FK_ArticleCommit_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleLicense",
                columns: table => new
                {
                    ArticleLicenseId = table.Column<long>(nullable: false),
                    ArticleId = table.Column<long>(nullable: false),
                    License = table.Column<string>(unicode: false, nullable: true),
                    LicensedDate = table.Column<DateTime>(type: "datetime2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleLicense", x => x.ArticleLicenseId);
                    table.ForeignKey(
                        name: "FK_ArticleLicense_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComment_ArticleId",
                table: "ArticleComment",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCommit_ArticleId",
                table: "ArticleCommit",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLicense_ArticleId",
                table: "ArticleLicense",
                column: "ArticleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleComment");

            migrationBuilder.DropTable(
                name: "ArticleCommit");

            migrationBuilder.DropTable(
                name: "ArticleLicense");

            migrationBuilder.DropTable(
                name: "Article");
        }
    }
}
