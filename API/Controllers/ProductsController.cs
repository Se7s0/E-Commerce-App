using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

//controller for extracting the data from the db
//the db will be the entity of products.cs, here we use code first

//Quick recap:
//A contoller is only resposible for handling HTTP requests, a context is responsible for db interactions(Queries)
//Here we used DI (dependency injection) to link the controller to this specific context (StoreContext) 
//we also make use of the lifetime of the context, in the startup we set the lifetime to scoped, meaning that the StoreContext will only be available 
//throught the HTTP request, upnon a new request, a new ProductController is init

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;

        }

        [HttpGet]
        //make use of action results from ControllerBase, its an HTTP response status
        public async Task<ActionResult<List<Product>>> GetProducts(){

            //here we made use of out StoreContext DbSet property "Products"
            //ToList executes a select query from our db and store results in this var products variable

            //an improvement to this method is making it async, HOW?
            //use a async jeyword in the method, add a Task to delegate the request until its finished, and use the ToListAsync

            var products = await _repo.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            return await _repo.GetProductByIDAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands(){

            return Ok(await _repo.GetProductBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductTypes(){

            return Ok(await _repo.GetProductTypesAsync());
        }
    }
}