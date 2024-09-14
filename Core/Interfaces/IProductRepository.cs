using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;


//we want to create a repositoty, we first create an interface to define the async methods we will use, 
//then we weill make the repo implement this interface

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIDAsync(int id);
        
        //a strongly written Readonly list, more specific in returning
        Task<IReadOnlyCollection<Product>> GetProductsAsync();
        Task<IReadOnlyCollection<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyCollection<ProductType>> GetProductTypesAsync();
    }
}