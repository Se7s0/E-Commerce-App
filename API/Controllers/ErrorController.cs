using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc;

//to handle any type of error like a not found endpoint
namespace API.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController 
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResonse(code));
        }
    }
}