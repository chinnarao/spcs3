using System;
using System.Collections.Generic;
using System.Linq;

namespace Share.Enums
{
    public static class EnumHelper
    {
        public static string GetEnumDisplayValue(Enum enumName)
        {
            var type = (Type)enumName.GetType();
            var field = type.GetField(enumName.ToString());
            if (field == null)
                return enumName.ToString();
            var display = ((System.ComponentModel.DataAnnotations.DisplayAttribute[])field.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)).FirstOrDefault();
            return display != null
                ? @display.GetName()
                : enumName.ToString();
        }

        //int categoryId = (byte)Enum.Parse(typeof(CategoryOptionsBy), CategoryName, true);
        //SortOptionsBy sort = (SortOptionsBy)Enum.Parse(typeof(SortOptionsBy), sortOptionsBy, true);


        //// TODO : Move it to Web Helpers 
        //public static List<SelectListItem> EnumToList(Type enumType, string selectedItem, bool defaultSelectItem = false)
        //{
        //    var items = new List<SelectListItem>();

        //    if (defaultSelectItem)
        //    {
        //        items.Add(new SelectListItem
        //        {
        //            Value = "",
        //            Text = "انتخاب کنید",
        //            Selected = string.IsNullOrWhiteSpace(selectedItem)
        //        });
        //    }

        //    if (enumType == null)
        //        return items;

        //    var values = Enum.GetValues(enumType);
        //    items.AddRange(from Enum item in values
        //                   select new SelectListItem
        //                   {
        //                       Value = item.ToString(),
        //                       Text = GetEnumDisplayValue(item),
        //                       Selected = selectedItem != null && item.ToString() == selectedItem.ToString()
        //                   });
        //    return items.ToList();
        //}

        public static List<EnumModel> EnumToList(Type enumType)
        {
            var items = new List<EnumModel>();

            var values = Enum.GetValues(enumType);
            items.AddRange(from Enum item in values
                           select new EnumModel
                           {
                               Value = Convert.ToInt32(item),
                               DisplayName = GetEnumDisplayValue(item),
                               Name = item.ToString()
                           });

            return items;
        }

    }
    public class EnumModel
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
