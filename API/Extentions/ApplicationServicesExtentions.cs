using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //choosing the lifetime, we have transient (method based mot request based) and sigleton(applkication based)
            //here added the products repo to our services container 
            services.AddScoped<IProductRepository, ProductRepository>(); 

            //here we added the generic repository to our services container, it is slightly different than the normal repo
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            
            services.Configure<ApiBehaviorOptions>(Options => 
            {
                //here we want to exreact the validation error from the modelstate as we mentioned in the ApiValidationErrorResponse
                //we want to overwrite what the behaviorr of the attribute does
                Options.InvalidModelStateResponseFactory = actionContext => 
                {
                    //extract the error message if any and populate the error messages into an array,
                    //this is the array which we will pass to apivalidationerrorresponse.cs error prop
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}