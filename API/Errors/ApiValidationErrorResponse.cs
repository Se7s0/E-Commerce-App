using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResonse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        //in the BaseApiController.cs, what is handling the validation errors is the 
        //attribute [ApiController], it makes us automatically know if there is a validation error instead of manually checking, adds it to
        //model state
        //we want to overwrite the behaviour of the apiController
        public IEnumerable<string> Errors { get; set; }
    }
}