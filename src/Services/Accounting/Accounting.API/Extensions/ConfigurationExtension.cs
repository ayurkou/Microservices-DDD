using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;

namespace Accounting.API.Extensions
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// format the version as "major[.minor][-status]"
        /// </summary>
        public static readonly string APIVersionPattern = "VVV";

        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: 
            services.AddMvcCore()
                .AddApiExplorer()
                .AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = $"'v'{APIVersionPattern}";//version as "'v'major[.minor][-status]"
                    options.ApiVersionParameterSource = new UrlSegmentApiVersionReader();
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.SubstituteApiVersionInUrl = true;
                    options.SubstitutionFormat = APIVersionPattern;
                });

            services.AddApiVersioning(
                opts =>
                {
                    opts.ReportApiVersions = true;
                });
            return services;
        }

        public static IServiceCollection ConfigureSwaggerDocs(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    // resolve the IApiVersionDescriptionProvider service
                    // note: that we have to build a temporary service provider here because one has not been created yet
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    // add a swagger document for each discovered API version
                    // note: you might choose to skip or document deprecated API versions differently
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }

                    // add a custom operation filter which sets default values
                    //options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);
                });
            return services;
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(ConfigurationExtension).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
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
