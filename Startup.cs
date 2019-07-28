using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using System.IO;
using graphql_demo.Resolvers;
using graphql_demo.Models;
using graphql_demo.Repository;
namespace graphql_demo
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;


        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this._logger = logger;
            this._environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddScoped<IRepository,InMemoryRepository>();
            // this enables you to use DataLoader in your resolvers.
            services.AddDataLoaderRegistry();

            // Add GraphQL Services
            var queryExecutionOptions = new QueryExecutionOptions
            {
                TracingPreference = TracingPreference.OnDemand
            };

            if (_environment.IsDevelopment())
            {
                queryExecutionOptions.TracingPreference = TracingPreference.Always;
                queryExecutionOptions.IncludeExceptionDetails = true;
                _logger.LogInformation(" *** LogInformation: In DEV Environment");

            }

            // Create our schema
            var schemaDef = File.ReadAllText("schema.graphql");
            var schema = Schema.Create(schemaDef, ConfigureSchema);

            services.AddGraphQL(schema, queryExecutionOptions);

        }

        private void ConfigureSchema(ISchemaConfiguration schema)
        {
            if (_environment.IsDevelopment())
            {
                schema.Options.StrictValidation = false;
            }

            schema.BindType<CreditCard>();

            schema.BindResolver<QueryResolver>()
                .To("Query")
                .Resolve("cards")
                .With(t => t.GetAllCardsAsync(default))
                .Resolve("card")
                .With(t => t.GetCardAsync(default, default))
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseGraphQL();
            app.UsePlayground();
        }
    }
}
