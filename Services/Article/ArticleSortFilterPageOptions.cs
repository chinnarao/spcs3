using System;
using Share.Enums;

namespace Services.Article
{
    public class ArticleSortFilterPageOptions
    {
        public OrderByOptions OrderByOptions { get; set; }

        public ArticlesFilterBy FilterBy { get; set; }

        public string FilterValue { get; set; }

        public int PageNumber { get; set; }

        public int DefaultPageSize { get; set; } //how many records should display in the screen

        public int TotalPagesBasedOnAvailableQueryDataRecords { get; private set; }

        /// <summary>
        /// This holds the state of the key parts of the SortFilterPage parts 
        /// </summary>
        public string PrevCheckState { get; set; }

        public void SetupRestOfDto(int count)
        {
            TotalPagesBasedOnAvailableQueryDataRecords = (int)Math.Ceiling((double)(count) / DefaultPageSize);
            PageNumber = Math.Min(Math.Max(1, PageNumber), TotalPagesBasedOnAvailableQueryDataRecords);

            var newCheckState = GenerateCheckState();
            if (PrevCheckState != newCheckState)
                PageNumber = 1;

            PrevCheckState = newCheckState;
        }

        /// <summary>
        /// This returns a string containing the state of the SortFilterPage data
        /// that, if they change, should cause the PageNum to be set back to 0
        /// </summary>
        /// <returns></returns>
        private string GenerateCheckState()
        {
            return $"{(int)FilterBy},{FilterValue},{DefaultPageSize},{TotalPagesBasedOnAvailableQueryDataRecords}";
        }
    }
}
