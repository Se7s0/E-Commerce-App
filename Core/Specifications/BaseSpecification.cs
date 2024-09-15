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

        //now we need a method that allows us to add include statements to our include lists, which is a list of expressions^^
        //protected as we plan to derive from this class
        protected void  AddInclude(Expression<Func<T, object>> includeExpression){
         Includes.Add(includeExpression);  
        }

    }
}