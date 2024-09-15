using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiResonse
    {
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        //contains the stack trace of the server side (500s) error
        //now we need to create middleware so we can handle exceptions and use this class 
        //in event when we get an exception
        public string Details { get; set; }
    }
}