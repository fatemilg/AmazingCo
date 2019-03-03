using AmazingCo.DataLayer;
using AmazingCo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AmazingCo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(
                    Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1
                );
            services
                .AddSingleton<ICompanyStructureDataLayer, InFileCompanyStructureDataLayer>();
            services.AddSingleton<ICompanyStructureService, InMemoryCompanyStructureServiceOptimizedForGet>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
