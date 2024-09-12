using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    //how we will query the databse
    public class StoreContext : DbContext
    {
        //a constructor to create the connection string, the connection string is passed to the base constructor, of Dbcontext
        // we will pass it the options type of StoreContext, as we will have other context types
        //each context represents a different database
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        //products is the name of the table that gets created when we generate the database
        //what is present in the products table is set in products.cs
        public DbSet<Product> Products {get; set;}
    }
}