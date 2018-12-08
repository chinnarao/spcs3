using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.AdMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ad",
                columns: table => new
                {
                    AdId = table.Column<long>(nullable: false),
                    AdTitle = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    AdContent = table.Column<string>(unicode: false, nullable: false),
                    AdCategoryId = table.Column<byte>(nullable: false),
                    AdDisplayDays = table.Column<byte>(nullable: false),
                    UserIdOrEmail = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    UserPhoneNumber = table.Column<long>(nullable: true),
                    UserPhoneCountryCode = table.Column<short>(nullable: true),
                    UserSocialAvatarUrl = table.Column<string>(unicode: false, maxLength: 5000, nullable: true),
                    UserSocialProviderName = table.Column<string>(unicode: false, maxLength: 12, nullable: true),
                    AddressStreet = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    AddressCity = table.Column<string>(unicode: false, maxLength: 60, nullable: true),
                    AddressDistrictOrCounty = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    AddressState = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    AddressPartiesMeetingLandmark = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    AddressZipCode = table.Column<string>(unicode: false, maxLength: 16, nullable: true),
                    AddressCountryCode = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    AddressCountryName = table.Column<string>(unicode: false, maxLength: 75, nullable: true),
                    AddressLatitude = table.Column<decimal>(type: "decimal(20, 10)", nullable: false, defaultValue: 1m),
                    AddressLongitude = table.Column<decimal>(type: "decimal(20, 10)", nullable: false, defaultValue: 1m),
                    AddressLocation = table.Column<IPoint>(nullable: false),
                    ItemCost = table.Column<double>(nullable: false, defaultValue: 0.0),
                    ItemCurrencyCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    ItemConditionId = table.Column<byte>(nullable: false),
                    AttachedAssetsInCloudCount = table.Column<byte>(nullable: false),
                    AttachedAssetsInCloudStorageId = table.Column<Guid>(nullable: true),
                    AttachedAssetsStoredInCloudBaseFolderPath = table.Column<string>(unicode: false, maxLength: 5000, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    IsPublished = table.Column<bool>(nullable: false, defaultValue: false),
                    PublishedDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    IsActivated = table.Column<bool>(nullable: false, defaultValue: false),
                    ActivatedDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Tag1 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag2 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag3 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag4 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag5 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag6 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag7 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag8 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag9 = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    Tag10 = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ad", x => x.AdId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ad");
        }
    }
}
