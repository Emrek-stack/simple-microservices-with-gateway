using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pantokrator.Order.Api.Middleware;
using Swashbuckle.AspNetCore.Swagger;

namespace Pantokrator.Order.Api
{
    public class Startup : WebApplicationStarter
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info { Title = "Pantokrator.Order.Api", Version = "v1" });
                c.DescribeStringEnumsInCamelCase();
                c.DescribeAllEnumsAsStrings();
            });

            return base.ConfigureServices(services);
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            //Put your dependencies (if you want)
            //builder.RegisterType<ProductRepository>().As<IProductRepository>();
            //builder.RegisterModule<ProductModule>();

            //Configuration Bind
            //Module Registration with parameter

            //MongoDb Module
            //builder.RegisterModule(new MongoDbModule
            //{
            //    ConnectionString = Configuration.GetConnectionString("MongoDbConnection")
            //});



            ////Services
            //builder.RegisterType<ValuesService>().As<IValuesService>().SingleInstance();

            ////Data
            //builder.RegisterType<ValuesRepository>().As<IValuesRepository>().SingleInstance();

            base.ConfigureContainer(builder);
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseTimeElapsed();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pantokrator.Order.Api");
            });

            base.Configure(app, env, loggerFactory);
        }
    }
}
