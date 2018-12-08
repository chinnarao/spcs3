using FluentValidation;
using Share.Models.Ad.Dtos;
using Share.Utilities;
using System;
using System.Text.RegularExpressions;

namespace Share._3rdParty
{
    public class AdDtoValidator : AbstractValidator<AdDto>
    {
        public AdDtoValidator()
        {
            RuleFor(x => x.AdId).MaximumLength(19);
            RuleFor(x => x.AdTitle).NotEmpty().Length(2, 500);
            RuleFor(x => x.AdContent).NotEmpty().MaximumLength(8000);
            RuleFor(x => x.AdCategoryId);
            RuleFor(x => x.AdDisplayDays).NotNull().GreaterThan<AdDto, Byte>(Byte.MinValue).LessThan<AdDto, Byte>(Byte.MaxValue);

            RuleFor(x => x.UserIdOrEmail).NotEmpty().MaximumLength(50);
            RuleFor(x => x.UserPhoneCountryCode).Must(IsValidCallingCode);
            //http://marcin-chwedczuk.github.io/fluent-validation-and-complex-dependencies-between-properties
            //RuleFor(x => x).Custom((dto, context) => {
            //    if (string.IsNullOrEmpty(dto?.UserPhoneCountryCode))
            //        return;
            //    int phoneNumber;
            //    bool ret = int.TryParse(dto.UserPhoneCountryCode, out phoneNumber);
            //    if (!ret)
            //        return;
            //    bool isValid = Utilities.Utility.IsValidCountryCallingCode(phoneNumber);
            //    if (isValid)
            //        return;
            //    if (!isValid)
            //        context.AddFailure(new ValidationFailure($"UserPhoneCountryCode", $"Phone Country Code is not a valid: ['{dto.UserPhoneCountryCode}']"));
            //});
            RuleFor(x => x.UserSocialAvatarUrl).MaximumLength(5000).Must(IsValidURL);
            RuleFor(x => x.UserSocialProviderName).MaximumLength(12);

            RuleFor(x => x.AddressStreet).MaximumLength(150);
            RuleFor(x => x.AddressCity).MaximumLength(60);
            RuleFor(x => x.AddressDistrictOrCounty).MaximumLength(30);
            RuleFor(x => x.AddressState).MaximumLength(30);
            RuleFor(x => x.AddressPartiesMeetingLandmark).MaximumLength(500);
            RuleFor(x => x.AddressZipCode).MaximumLength(16);
            RuleFor(x => x.AddressCountryCode).MaximumLength(2);
            RuleFor(x => x.AddressCountryName).MaximumLength(75);
            RuleFor(x => x.AddressLatitude).Must(Utility.IsValidLatitude);
            RuleFor(x => x.AddressLongitude).Must(Utility.IsValidLongitude);

            RuleFor(x => x.ItemCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ItemCurrencyCode).MaximumLength(3);
            RuleFor(x => x.AttachedAssetsInCloudStorageId).NotEqual(Guid.Empty);
            RuleFor(x => x.AttachedAssetsStoredInCloudBaseFolderPath).MaximumLength(5000).Must(IsValidURL);

            RuleFor(x => x.Tag1).MaximumLength(20);
            RuleFor(x => x.Tag2).MaximumLength(20);
            RuleFor(x => x.Tag3).MaximumLength(20);
            RuleFor(x => x.Tag4).MaximumLength(20);
            RuleFor(x => x.Tag5).MaximumLength(20);
            RuleFor(x => x.Tag6).MaximumLength(20);
            RuleFor(x => x.Tag7).MaximumLength(20);
            RuleFor(x => x.Tag8).MaximumLength(20);
            RuleFor(x => x.Tag9).MaximumLength(20);
            RuleFor(x => x.Tag10).MaximumLength(20);
        }

        //http://urlregex.com/
        private bool IsValidURL(string URL)
        {
            if (String.IsNullOrWhiteSpace(URL))
                return true;
            Regex regex = new Regex(Utilities.RegexUtility.GetUrlRegex());
            Match match = regex.Match(URL);
            return match.Success;
        }

        private bool IsValid10DigitPhoneNumber(string phone)
        {
            if (String.IsNullOrWhiteSpace(phone))
                return true;
            return phone.Length == 10;
        }

        private bool IsValidCallingCode(string code)
        {
            if (String.IsNullOrWhiteSpace(code))
                return true;
            short s;
            if (short.TryParse(code, out s))
                return s > 0 && s <= 995;
            return false;
        }

        ////-180.0 to 180.0.
        //private bool IsValidLongitude(string longitude)
        //{
        //    if (string.IsNullOrWhiteSpace(longitude))
        //        return true;
        //    double l;
        //    if (double.TryParse(longitude, out l))
        //    {
        //        if (l < -180 || l > 180)
        //            return false;
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        ////-90.0 to 90.0.
        //private bool IsValidLatitude(string latitude)
        //{
        //    if (string.IsNullOrWhiteSpace(latitude))
        //        return true;
        //    double l;
        //    if (double.TryParse(latitude, out l))
        //    {
        //        if (l < -90 || l > 90)
        //            return false;
        //        return true;
        //    }
        //    else
        //        return false;
        //}
    }
}
