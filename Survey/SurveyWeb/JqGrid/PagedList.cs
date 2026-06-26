using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyWeb.JqGrid
{
    public interface IPagedList
    {

        int TotalCount
        {
            get;
            set;
        }

        int PageIndex
        {
            get;
            set;
        }

        int PageSize
        {
            get;
            set;
        }

        bool IsPreviousPage
        {
            get;
        }

        bool IsNextPage
        {
            get;
        }
    }
    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            try
            {
                this.TotalCount = source.Count();
                this.PageSize = pageSize;
                this.PageIndex = index;
                this.AddRange(source.Skip(index).Take(pageSize).ToList());
            }
            catch (NotSupportedException )
            {
                this.AddRange(source.ToList());
            }
        }
        public PagedList(IQueryable<T> source)
        {
            try
            {
                this.TotalCount = source.Count();
                this.AddRange(source.ToList());
            }
            catch (NotSupportedException )
            {
                this.AddRange(source.ToList());
            }
        }
        public PagedList()
        {

        }
        public PagedList(List<T> source)
        {
            this.AddRange(source.ToList());
        }
        public PagedList(List<T> source, int index, int pageSize)
        {
            this.TotalCount = source.Count();
            this.PageSize = pageSize;
            this.PageIndex = index;
            if (index == -1 || pageSize == -1)
            {
                this.AddRange(source.ToList());
            }
            else
            {
                this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
            }
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public bool IsPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool IsNextPage
        {
            get
            {
                return (PageIndex * PageSize) <= TotalCount;
            }
        }
    }

    public static class Pagination
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, GridSettings gridBusinessSetting)
        {
            if (!gridBusinessSetting.LoadAll)
                return new PagedList<T>(source, (gridBusinessSetting.PageIndex - 1) * gridBusinessSetting.PageSize, gridBusinessSetting.PageSize);
            return new PagedList<T>(source);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index)
        {
            return new PagedList<T>(source, index, 10);
        }
    }
}
