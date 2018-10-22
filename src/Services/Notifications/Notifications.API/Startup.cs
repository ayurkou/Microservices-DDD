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
using System.Linq;
using System.Reflection;
using Notifications.Api.AutofacModules;
using Notifications.API.Consumers;
using Notifications.API.Extensions;
using Notifications.API.Hubs;

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
            services.AddSwaggerWithApiVersionig();
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();
      
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new RabbitMqModule(Config.BusConfig));
            containerBuilder.RegisterModule(new LoggerModule());
            containerBuilder.RegisterModule(new ConsumersModule());
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
                foreach (var d in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{d.GroupName}/swagger.json", d.GroupName.ToUpperInvariant());
                }
            });

            app.UseSignalR(b => b.MapHub<NotificationHub>($"/{nameof(NotificationHub)}"));
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
