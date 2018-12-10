using NetTopologySuite.Geometries;
using System;

namespace Share.Models.Ad.Entities
{
    public class Ad
    {
        public Int64 AdId { get; set; }  //http://www.talkingdotnet.com/use-hilo-to-generate-keys-with-entity-framework-core/
        public string AdTitle { get; set; }
        public string AdContent { get; set; }   // product details , including category
        public byte AdCategoryId { get; set; }
        public byte AdDisplayDays { get; set; }       

        public string UserIdOrEmail { get; set; }    
        public long? UserPhoneNumber { get; set; }
        public short? UserPhoneCountryCode { get; set; }
        public string UserSocialAvatarUrl { get; set; }
        public string UserSocialProviderName { get; set; } //ex: facebook, twitter

        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }     // *****
        public string AddressDistrictOrCounty { get; set; }
        public string AddressState { get; set; }
        public string AddressPartiesMeetingLandmark { get; set; }  
        public string AddressZipCode { get; set; }   //http://www.zipinfo.com/products/zcug/zcug.htm    // *****
        public string AddressCountryCode { get; set; }   // *****
        public string AddressCountryName { get; set; }    
        public double AddressLatitude { get; set; }
        public double AddressLongitude { get; set; }
        public Point AddressLocation { get; set; }

        public double ItemCost { get; set; }   //enter with currency code ex: dollar or rupees   // *****
        public string ItemCurrencyCode { get; set; }  // https://www.countries-ofthe-world.com/world-currencies.html
        public byte ItemConditionId { get; set; }     //old or new   // *****

        public byte AttachedAssetsInCloudCount { get; set; }   
        public Guid? AttachedAssetsInCloudStorageId { get; set; }
        public string AttachedAssetsStoredInCloudBaseFolderPath { get; set; }
        
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDateTime { get; set; }                
        public bool IsActivated { get; set; }                                  
        public DateTime? ActivatedDateTime { get; set; }                   

        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string Tag6 { get; set; }
        public string Tag7 { get; set; }
        public string Tag8 { get; set; }
        public string Tag9 { get; set; }
        public string Tag10 { get; set; }
    }
}
