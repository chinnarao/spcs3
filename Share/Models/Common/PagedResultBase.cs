using System;

//https://gunnarpeipman.com/net/ef-core-paging/
namespace Share.Models.Common
{
    public abstract class PagedResultBase
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        //public int FirstRowOnPage
        //{
        //    get { return (Page - 1) * PageSize + 1; }
        //}

        //public int LastRowOnPage
        //{
        //    get { return Math.Min(Page * PageSize, RowCount); }
        //}
    }
}
