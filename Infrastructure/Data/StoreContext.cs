using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
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
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        //here we specify that we want to get the configutations from the ProductConfiguration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType 
                    == typeof(decimal));

                    foreach (var property in properties) 
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion<double>();
                    }


                }
            }
        }

    }
}