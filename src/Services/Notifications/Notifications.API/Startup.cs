using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;
using Notifications.Api.AutofacModules;
using Notifications.API.Consumers;

namespace Notifications.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Config = new AppConfig(configuration);
        }

        public AppConfig Config { get; }
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
//            #region API Versioning
//            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
//            // note: the specified format code will format the version as "'v'major[.minor][-status]"
//            services.AddMvcCore()
//                .AddApiExplorer()
//                .AddVersionedApiExplorer(
//                options =>
//                {
//                    options.GroupNameFormat = "'v'VVV";
//
//                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
//                    // can also be used to control the format of the API version in route templates
//                    options.ApiVersionParameterSource = new UrlSegmentApiVersionReader();
//                    options.AssumeDefaultVersionWhenUnspecified = true;
//                    options.DefaultApiVersion = new ApiVersion(1, 0);
//                    options.DefaultApiVersionParameterDescription = "default";
//                    options.SubstituteApiVersionInUrl = true;
//                    options.SubstitutionFormat = "VVV";
//                });
//
//            services.AddApiVersioning(
//                opts =>
//                {
//                    opts.ReportApiVersions = true;
//                    opts.AssumeDefaultVersionWhenUnspecified = true;
//                    opts.DefaultApiVersion = new ApiVersion(1, 0);
//                });
//
//            #endregion API Versioning

            services
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

//            services.AddSwaggerGen(
//                options =>
//                {
//                    // resolve the IApiVersionDescriptionProvider service
//                    // note: that we have to build a temporary service provider here because one has not been created yet
//                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
//
//                    // add a swagger document for each discovered API version
//                    // note: you might choose to skip or document deprecated API versions differently
//                    foreach (var description in provider.ApiVersionDescriptions)
//                    {
//                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
//                    }
//
//                    // add a custom operation filter which sets default values
//                    //options.OperationFilter<SwaggerDefaultValues>();
//
//                    // integrate xml comments
//                    options.IncludeXmlComments(XmlCommentsFilePath);
//                });       

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new RabbitMqModule(Config.BusConfig));
            containerBuilder.RegisterModule(new LoggerModule());
            containerBuilder.RegisterModule(new ConsumersModule());
            containerBuilder.Populate(services);
            Container = containerBuilder.Build();
            return new AutofacServiceProvider(Container);
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
                app.UseHsts();
            }
            
//            app.UseSwagger();
//            app.UseSwaggerUI(c =>
//            {
//                c.RoutePrefix = string.Empty;
//                
//                // build a swagger endpoint for each discovered API version
//                foreach (var description in provider.ApiVersionDescriptions)
//                {
//                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
//                }
//            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"Daytona Accounting API {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
