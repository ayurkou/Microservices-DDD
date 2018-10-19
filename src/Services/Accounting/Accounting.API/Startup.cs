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
using Accounting.Api.AutofacModules;
using Accounting.API.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Accounting.Api
{
    public class Startup
    {
        public AppConfig Config { get; }

        public IContainer Container { get; private set; }


        public Startup(IConfiguration configuration)
        {
            Config = new AppConfig(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerWithApiVersionig();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ///UI client side
            //// Allow sign in via an OpenId Connect provider
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})
            //.AddOpenIdConnect(options =>
            //{
            //    options.ClientId = "07eceae0-b522-0136-7fe6-06a642e08480136373";
            //    options.ClientSecret = "9dd5bd0da5adb58c131ed5b431d522712d2ac015cc93c93322ed6ecfb5c465e3";
            //    options.Authority = "https://solera-dev.onelogin.com/oidc";
            //    options.ResponseType = "code";
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //}
            //);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://solera-dev.onelogin.com/oauth2/default";
                options.Audience = "Daytona";
                //options.TokenValidationParameters
            });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new RabbitMqModule(Config.BusConfig));
            containerBuilder.RegisterModule(new LoggerModule());
            containerBuilder.Populate(services);
            Container = containerBuilder.Build();
            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Daytona Accounting API {description.GroupName}");
                }
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
