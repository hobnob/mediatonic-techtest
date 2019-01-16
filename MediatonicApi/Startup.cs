using MediatonicApi.Models;
using MediatonicApi.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace MediatonicApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(o => {
                o.SwaggerDoc("v1", new Info {
                    Version = "v1",
                    Title = "Mediatonic Pets API",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "api.xml");
                o.IncludeXmlComments(xmlPath);
            });

            // Set up the db context
            services.AddDbContext<ApiContext>(options =>
               options.UseSqlite(Configuration.GetConnectionString("dbConnection")));

            // Set up the services for a request
            services.AddScoped<IService<User>, UserService>();
            services.AddScoped<IService<Animal>, AnimalService>();
            services.AddScoped<IService<UserAnimal>, UserAnimalService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(o => {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "Mediatonic Pets API");
            });
        }
    }
}
