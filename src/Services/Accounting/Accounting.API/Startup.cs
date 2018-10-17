using System;
using accounting.api.AutofacModules;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace accounting.api
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
            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info {Title = "Daytona Accounting API", Version = "v1"});
                    c.DescribeAllEnumsAsStrings();
                });
            services
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new RabbitMqModule(Config.BusConfig));
            containerBuilder.RegisterModule(new LoggerModule());
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Daytona Accounting API V1");
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
