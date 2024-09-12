using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context; 
        }

        [HttpGet]
        //make use of action results from ControllerBase, its an HTTP response status
        public async Task<ActionResult<List<Product>>> GetProducts(){

            //here we made use of out StoreContext DbSet property "Products"
            //ToList executes a select query from our db and store results in this var products variable

            //an improvement to this method is making it async, HOW?
            //use a async jeyword in the method, add a Task to delegate the request until its finished, and use the ToListAsync

            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            return await _context.Products.FindAsync(id);
        }
    }
}