using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbContexts.Ad
{
    //https://www.learnentityframeworkcore.com/configuration/fluent-api
    //https://github.com/loresoft/InstructorIQ/tree/5497c8324cd256f061981ef4ecbed0fcaca0151d/service/src/InstructorIQ.Core/Data/Mapping
    public class AdConfig : IEntityTypeConfiguration<Share.Models.Ad.Entities.Ad>
    {
        public void Configure(EntityTypeBuilder<Share.Models.Ad.Entities.Ad> e)
        {
            e.ToTable("Ad");
            e.Property(p => p.AdId).ValueGeneratedNever();  //ForSqlServerUseSequenceHiLo    //UseSqlServerIdentityColumn()
            e.Property(p => p.AdTitle).IsRequired().IsUnicode(false).HasMaxLength(500);
            e.Property(p => p.AdContent).IsRequired().IsUnicode(false);
            e.Property(p => p.AdCategoryId).IsRequired();
            e.Property(p => p.AdDisplayDays).IsRequired();

            e.Property(p => p.UserIdOrEmail).IsRequired().IsUnicode(false).HasMaxLength(50);
            e.Property(p => p.UserPhoneNumber);
            e.Property(p => p.UserPhoneCountryCode);
            e.Property(p => p.UserSocialAvatarUrl).IsUnicode(false).HasMaxLength(5000);
            e.Property(p => p.UserSocialProviderName).IsUnicode(false).HasMaxLength(12);   //facebook or local

            e.Property(p => p.AddressStreet).IsUnicode(false).HasMaxLength(150);
            e.Property(p => p.AddressCity).IsUnicode(false).HasMaxLength(60);
            e.Property(p => p.AddressDistrictOrCounty).IsUnicode(false).HasMaxLength(30);
            e.Property(p => p.AddressState).IsUnicode(false).HasMaxLength(30);
            e.Property(p => p.AddressPartiesMeetingLandmark).IsUnicode(false).HasMaxLength(500);
            e.Property(p => p.AddressZipCode).IsUnicode(false).HasMaxLength(16);
            e.Property(p => p.AddressCountryCode).IsUnicode(false).HasMaxLength(2);
            e.Property(p => p.AddressCountryName).IsUnicode(false).HasMaxLength(75);
            e.Property(p => p.AddressLatitude).HasColumnType("decimal(20, 10)").HasDefaultValue<double>(1.0);
            e.Property(p => p.AddressLongitude).HasColumnType("decimal(20, 10)").HasDefaultValue<double>(1.0);
            e.Property(p => p.AddressLocation).IsRequired();

            e.Property(p => p.ItemCost).HasDefaultValue<double>(0.0);
            e.Property(p => p.ItemCurrencyCode).IsUnicode(false).HasMaxLength(3);
            e.Property(p => p.ItemConditionId).IsRequired();

            e.Property(p => p.AttachedAssetsInCloudCount);
            e.Property(p => p.AttachedAssetsInCloudStorageId);
            e.Property(p => p.AttachedAssetsStoredInCloudBaseFolderPath).IsUnicode(false).HasMaxLength(5000);
            
            e.Property(p => p.CreatedDateTime).IsRequired().HasColumnType("datetime2(7)"); // conflict : sqlite and sql server.HasDefaultValueSql("date('now')"); //"getdate()"  or "(SYSDATETIME())"  or GetUtcDate(), [[ worked succes with this date('now'), the reason, this sql lite fn, testinf purpose]]
            e.Property(p => p.UpdatedDateTime).IsRequired().HasColumnType("datetime2(7)");
            e.Property(p => p.IsDeleted).IsRequired().HasDefaultValue<bool>(false);
            e.Property(p => p.DeletedDateTime).HasColumnType("datetime2(7)");
            e.Property(p => p.IsPublished).IsRequired().HasDefaultValue<bool>(false);
            e.Property(p => p.PublishedDateTime).HasColumnType("datetime2(7)");
            e.Property(p => p.IsActivated).IsRequired().HasDefaultValue<bool>(false);
            e.Property(p => p.ActivatedDateTime).HasColumnType("datetime2(7)");

            e.Property(p => p.Tag1).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag2).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag3).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag4).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag5).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag6).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag7).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag8).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag9).IsUnicode(false).HasMaxLength(20);
            e.Property(p => p.Tag10).IsUnicode(false).HasMaxLength(20);
        }
    }
}
