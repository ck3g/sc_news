using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCNews.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize, int recordsCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            //TotalCount = source.Count();
            TotalCount = recordsCount;
            TotalPages = (int)Math.Ceiling( TotalCount / (double)PageSize ) + 1;
            //this.AddRange( source.Skip( (PageIndex - 1) * PageSize ).Take( PageSize ) );
            this.AddRange(source);
        }

        public bool HasPreviousPage { get { return ( PageIndex > 1 ); } }

        public bool HasNextPage { get { return ( PageIndex + 1 < TotalPages ); } }
    }
}