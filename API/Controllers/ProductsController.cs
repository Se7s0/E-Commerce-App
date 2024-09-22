using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

//controller for extracting the data from the db
//the db will be the entity of products.cs, here we use code first

//Quick recap:
//A contoller is only resposible for handling HTTP requests, a context is responsible for db interactions(Queries)
//Here we used DI (dependency injection) to link the controller to this specific context (StoreContext) 
//we also make use of the lifetime of the context, in the startup we set the lifetime to scoped, meaning that the StoreContext will only be available 
//throught the HTTP request, upnon a new request, a new ProductController is init

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo, 
            IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }
 
        [HttpGet]
        //make use of action results from ControllerBase, its an HTTP response status
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            //the contoller is not able to automaticcaly bind the paramaters as we used an objedct, so we use a fromQuery attribute
            //explicitylu bind from the query string : {{url}}/api/products?typeId=3&brandId=2
            [FromQuery] ProductSpecParams productParams){

            //here we made use of out StoreContext DbSet property "Products"
            //ToList executes a select query from our db and store results in this var products variable

            //an improvement to this method is making it async, HOW?
            //use a async jeyword in the method, add a Task to delegate the request until its finished, and use the ToListAsync

            //this method now has a spec and uses a generic repo, we  created a spec and passed it to the function we specified
            
            //////******************///////////////////
            //ListAsync, which uses a local functtion ApplySpecification and TRACE, TRACE AGAIN IN CASE U FORGET how it works
            //////******************///////////////////
 
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var products = await _productsRepo.ListAsync(spec);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
          
        //calls the constructor with the param
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){

            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product =  await _productsRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResonse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands(){

            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductTypes(){

            return Ok(await _productTypeRepo.ListAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductDto productDto)
        {
            var product = _mapper.Map<ProductDto, Product>(productDto);

            _productsRepo.Add(product);
            await _productsRepo.SaveAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepo.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _productsRepo.Delete(product);
            await _productsRepo.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductPrice(int id, [FromBody] int newPrice)
        {
            var product = await _productsRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            product.Price = newPrice;
            _productsRepo.Update(product);
            await _productsRepo.SaveAsync();

            return NoContent();
        }

        
    }
}