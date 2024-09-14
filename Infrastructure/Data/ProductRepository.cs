using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    //implement the interface 
    //the repo is going to be interacting with the StoreContext to get the data from the databse
    //the repo itself contains the linq queries to be used by the context
    //then the controller is going to be using the repository methods to retrieve data from the databse,
    //the store context handles the the databse interactions 

    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIDAsync(int id)
        {
            return await _context.Products
            .Include(p=> p.ProductType)
            .Include(p=> p.ProductBrand)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyCollection<Product>> GetProductsAsync()
        {
            return await _context.Products
            .Include(p=> p.ProductType)
            .Include(p=> p.ProductBrand)
            .ToListAsync();
        }

        public async Task<IReadOnlyCollection<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}