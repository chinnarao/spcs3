using Share.Models.Ad.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Share.Extensions
{
    public static class ModelsExtensions
    {
        public static List<AdDto> ToAdDtoList<T>(IQueryable<T> list) 
        {
            //var template = Scriban.Template.Parse(html);
            //if (template.HasErrors)
            //    throw new Exception(string.Join<Scriban.Parsing.LogMessage>(',', template.Messages.ToArray()));
            //string result = template.Render(anonymousDataObject);
            //return result;

            return null;
        }
    }
}
