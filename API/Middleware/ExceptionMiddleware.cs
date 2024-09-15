using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

//middleware to handle the exeptions and redirect to the ApiExceptions.cs
namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        //TODO: understand the ctor parameters and why they are needed
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        //
        public async Task InvokeAsync(HttpContext context)
        {
            //a request delegate is a fucntion that can handle Http requests
            //now if there is no exception we want the middleware to move to the next middleware
            try
            {
                //this means that if there is no exception the request moves on to its next stage
                await _next(context);
            }

            //if there is an exceotion, we want to catch it, and we want to have our own exception handling response
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message); //output to the logging console
                context.Response.ContentType = "application/json"; //we want the responses to be json formatted
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; //set the status code to be a server error

                var response = _env.IsDevelopment() //check if we are indevelopment or production to see if we ouput stacktrace or not
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message,
                    ex.StackTrace.ToString())
                    :new ApiException((int) HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response, options); 
                await context.Response.WriteAsync(json);

                //now we want to add this middleware in the pipeline
            }
        }

    }
}