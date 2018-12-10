using FluentValidation;
using Share.Models.Ad.Dtos;
using Share.Utilities;

namespace Share._3rdParty
{
    public class AdSearchDtoValidator : AbstractValidator<AdSearchDto>
    {
        public AdSearchDtoValidator()
        {
            RuleFor(x => x.SearchText).MaximumLength(100);
            RuleFor(x => x.CountryCode).MaximumLength(2);
            RuleFor(x => x.CityName).MaximumLength(60);
            RuleFor(x => x.ZipCode).MaximumLength(16);
            RuleFor(x => x.CurrencyCode).MaximumLength(3);
            RuleFor(x => x.MapLatitude).Must(Utility.IsValidLatitude);
            RuleFor(x => x.MapLongitude).Must(Utility.IsValidLongitude);
        }
    }
}
