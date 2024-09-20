using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

//this is going to take the specifications and evaluate them to return an IQueryable for the ToListAsync() method

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {

            var query = inputQuery;

            // Apply criteria (filtering) if available in the specification
            if(spec.Criteria != null){
                query = query.Where(spec.Criteria);
            }            
            
            //after we applyinf them in the evaluator, we now need to catch the specs from the client
            //we do this in the products controller
            if(spec.OrderBy != null){
                query = query.OrderBy(spec.OrderBy);
            }           
            if(spec.OrderByDescending != null){
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if(spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }


            // Apply Includes (for eager loading) from the specification
            //The Aggregate method iterates through each Include expression and applies it to the query
            //This mimics the behavior of using .Include() in a non-generic repository.

            //TODO: refer to manual for more info about the aggregate and its uses
            //briefly, it just loops around the include statements and filters the query with the current expression until the end of the list
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}