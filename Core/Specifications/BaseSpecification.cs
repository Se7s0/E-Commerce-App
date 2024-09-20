using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


//here the specification is used to add includes to a generic repo, egenric repos them selves do not support include statements to 
//include certain fields in the query

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {

        //here we provided 2 constructors to handle both get products cases, the one with criteria(get by id)
        //and the other where we list all products

        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        //Expression takes a function, a fucntion takes a type --T and what its returning --bool
        //we are specifing the criteria of what we are going to get 
        //this is like a LINQ "where" criteria is
        public Expression<Func<T, bool>> Criteria {get;}

        //this is the list of our include statements that we are going to pass to out ToListAsync() method
        //initialize to an empty list so we can populate it
        public List<Expression<Func<T, object>>> Includes {get;} = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy {get; private set;}

        public Expression<Func<T, object>> OrderByDescending {get; private set;}

        //now we need a method that allows us to add include statements to our include lists, which is a list of expressions^^
        //protected as we plan to derive from this class
        protected void AddInclude(Expression<Func<T, object>> includeExpression){
         Includes.Add(includeExpression);  
        }

        //these methods need to be evaluated by our spec eval so it can get added to out
        //queryable and be then return and passed to our method of ToList
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        //add methods to set the paging properties
        public int Take {get; private set;}

        public int Skip {get; private set;}

        public bool IsPagingEnabled {get; private set;}

        protected void ApplyPaging(int skip, int take)
        {
            //example scenario on skip and take:
            //Skip skips a number of records typically when on a page, so for example each page shows 10 items, 
            //we on page 3, so skip will be 20 and take will always be 10
            //skip value is typically = ((pageNumber - 1) * pageSize)
            Skip = skip;
            Take = take;
            
            //we will use this prop in our sopec evaluator to see wether or not to apply paging
            IsPagingEnabled = true;
        }

    }
}