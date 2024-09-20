using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria {get;}
        List<Expression<Func<T, object>>> Includes{get;}

        //as we now have a generic repo, we need to define our orderby fucntions, if we had a non generic repo
        //we could use the function orderby directly to query the db
        Expression<Func<T, Object>> OrderBy{get; }
        Expression<Func<T, Object>> OrderByDescending{get; }

        //take a certain amound of products
        //skip a certain amount of products
        //is paging enabled
        int Take {get;}
        int Skip {get;}
        bool IsPagingEnabled{get;}

    }
}