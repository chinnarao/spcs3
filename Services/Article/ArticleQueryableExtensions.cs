using Share.Models.Article.Dtos;
using System;
using System.Linq;
using Share.Enums;

namespace Services.Article
{
    public static partial class ArticleQueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNumZeroStart, int pageSize)
        {
            if (pageSize == 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "pageSize cannot be zero.");

            if (pageNumZeroStart != 0)
                query = query.Skip(pageNumZeroStart * pageSize);

            return query.Take(pageSize);
        }

        public const string AllArticlesNotPublishedString = "Coming Soon";
        /***************************************************************
        #A The method is given both the type of filter and the user selected filter value
        #B If the filter value isn't set then it returns the IQueryable with no change
        #C Same for no filter selected - it returns the IQueryable with no change
        #D The filter by votes is a value and above, e.g. 3 and above. Note: not reviews returns null, and the test is always false
        #E If the "coming soon" was picked then we only return books not yet published
        #F If we have a specific year we filter on that. Note that we also remove future books (in case the user chose this year's date)
         * ************************************************************/
        public static IQueryable<ArticleDto> FilterArtilclesBy(this IQueryable<ArticleDto> articles, ArticlesFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return articles;

            switch (filterBy)
            {
                case ArticlesFilterBy.NoFilter:
                    return articles;
                case ArticlesFilterBy.ByVotes:
                    var filterVote = int.Parse(filterValue);
                    return articles.Where(x => x.ArticleAverageVotes > filterVote);
                case ArticlesFilterBy.ByPublicationYear:
                    if (filterValue == AllArticlesNotPublishedString)
                        return articles.Where(x => x.PublishedDate > DateTime.UtcNow);
                    var filterYear = int.Parse(filterValue);
                    return null;
                    //return articles.Where(x => x.PublishedDate.Year == filterYear && x.PublishedDate <= DateTime.UtcNow);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        /************************************************************
        #A Because of paging we always need to sort. I default to showing latest entries first
        #B This orders the book by votes. Books without any votes (null return) go at the bottom
        #C Order by publication date - latest books at the top
        #D Order by actual price, which takes into account any promotional price - both lowest first and highest first
         * ********************************************************/
        public static IQueryable<ArticleDto> OrderArticlesBy(this IQueryable<ArticleDto> articles, OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.NoOrder:
                    return articles.OrderByDescending(x => x.ArticleId);
                //case OrderByOptions.ByVotes:
                //    return books.OrderByDescending(x => x.ReviewsAverageVotes);
                case OrderByOptions.ByPublicationDate:
                    return articles.OrderByDescending(x => x.PublishedDate);
                //case OrderByOptions.ByPriceLowestFirst:
                //    return books.OrderBy(x => x.ActualPrice);
                //case OrderByOptions.ByPriceHigestFirst:
                //    return books.OrderByDescending(x => x.ActualPrice);
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
