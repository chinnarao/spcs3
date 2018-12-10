using Share.Models.Ad.Dtos;
using System;

namespace Share._3rdParty
{
    public static class ScribanExtension
    {
        // this function may not work with dynamic
        //Extension methods cannot be dynamically dispatched. Consider casting the dynamic arguments or calling the extension method without the extension method syntax.
        public static string ToParseLiquidRender(this string html, AdDto dto)
        {
            var anonymousDataObject =  new
            {
                activedays = dto.AdDisplayDays,
                adaddressatpublicsecuritynearlandmarkname = dto.AddressPartiesMeetingLandmark,
            };

            var template = Scriban.Template.Parse(html);
            if (template.HasErrors)
                throw new Exception(string.Join<Scriban.Parsing.LogMessage>(',', template.Messages.ToArray()));
            string result = template.Render(anonymousDataObject);
            return result;
        }
    }
}
