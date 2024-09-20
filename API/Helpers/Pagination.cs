using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//this class is used to wrap our response around and return any pagination info that the client can make use of
namespace API.Helpers
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        //count of items after filter applied
        //the paging in the spec class can return a count, this count is after paging,
        //we need to implement something that counts the total number of items before paging
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}