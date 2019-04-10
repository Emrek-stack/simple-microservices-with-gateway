using System;
using System.IO.Compression;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pantokrator.Order.Api.Filters;

namespace Pantokrator.Order.Api
{
    public class WebApplicationStarter
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }


        protected WebApplicationStarter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    o.InputFormatters.RemoveType<XmlDataContractSerializerInputFormatter>();
                    o.InputFormatters.RemoveType<XmlSerializerInputFormatter>();

                    o.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                    o.OutputFormatters.RemoveType<StreamOutputFormatter>();
                    o.OutputFormatters.RemoveType<StringOutputFormatter>();
                    o.OutputFormatters.RemoveType<XmlDataContractSerializerOutputFormatter>();
                    o.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();

                    o.Filters.Add<ValidateModelStateFilter>();
                    o.Filters.Add<GlobalExceptionFilter>();
                })
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.Formatting = Formatting.Indented;
                    o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    o.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddCors()
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression();

            var builder = new ContainerBuilder();
            //populate built-in DI and Registered Autofac dependencies
            builder.Populate(services);

            //Build Container
            ConfigureContainer(builder);
            return new AutofacServiceProvider(ApplicationContainer);
        }


        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            ApplicationContainer = builder.Build();
        }



        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();
            app.UseCors(
                options => options.AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
            );

            app.UseMvc();
        }
    }
}