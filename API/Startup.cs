using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API.Extentions;

namespace API
{
    public class Startup
    {
        //here we specify the startup configuration for the StoreContext by soecifying the connection string and specifyong the dbcontext
        //now we want to apply migrations to build the db (reads from the Dbset) 
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));

            // //choosing the lifetime, we have transient (method based mot request based) and sigleton(applkication based)
            // //here added the products repo to our services container 
            // services.AddScoped<IProductRepository, ProductRepository>(); 

            // //here we added the generic repository to our services container, it is slightly different than the normal repo
            // services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            services.AddControllers();
            //this service needs to be added after AddController service
            // services.Configure<ApiBehaviorOptions>(Options => 
            // {
            //     //here we want to exreact the validation error from the modelstate as we mentioned in the ApiValidationErrorResponse
            //     //we want to overwrite what the behaviorr of the attribute does
            //     Options.InvalidModelStateResponseFactory = actionContext => 
            //     {
            //         //extract the error message if any and populate the error messages into an array,
            //         //this is the array which we will pass to apivalidationerrorresponse.cs error prop
            //         var errors = actionContext.ModelState
            //             .Where(e => e.Value.Errors.Count > 0)
            //             .SelectMany(x => x.Value.Errors)
            //             .Select(x => x.ErrorMessage).ToArray();
            //         var errorResponse = new ApiValidationErrorResponse
            //         {
            //             Errors = errors
            //         };
            //         return new BadRequestObjectResult(errorResponse);
            //     };
            // });
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            // });
            services.AddDbContext<StoreContext>(x=>x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
            services.AddApplicationServices();
            services.AddSwaggerDocumentation();
            services.AddCors(opt =>
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                })
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //we need to replace this middleware by our own
            //to handle errors while in production mode -- understand more about this
            app.UseMiddleware<ExceptionMiddleware>();

            // app.UseSwagger();
            // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            //redirect to error controller
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
