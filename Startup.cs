using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo1.Models;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreDemo1
{
    public class Startup
    {
        //created constructor on 1.1.2020 we need IConfiguration to read the key values from appsetting.json file
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            this._config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region adding dbcontext service
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            #endregion
            #region AddMvc() Method
            //AddMvc method internally calls AddMvcCore services so no need to call AddMvcCore explicitly
            // to get json data in xml format we add 'AddXmlSerializerFormatters' method
            services.AddMvc().AddXmlSerializerFormatters();
            
            #endregion
            //adding dependency injection
            //services.AddTransient<IEmployeeRpository, MockEmployeeRepository>();//this is using MockEmployeeRepository with interface IEmployeeRpository
            services.AddScoped<IEmployeeRpository, SQLEmployeeRepository>();//this is using SQLEmployeeRepository with interface IEmployeeRpository
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region use the 404 error middleware apart from development environment
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            #endregion
            //app.Run(async (context) =>
            //{
            //    // System.Diagnostics.Process.GetCurrentProcess().ProcessName to read current process like iisexpress,dotnet
            //    //await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

            //    //Reading the value for key 'MyKey' from appsetting.json    
            // it will be overriden as launchsetting->appsetting.development->appsetting                                             
            //    await context.Response.WriteAsync(_config["MyKey"]);
            //});

            //below are two middlewares being called but will run once only for first one 
            // to use second one we need to include use method in stead of run as below
            //app.Run(async (context) =>
            //{   
            //    await context
            //    .Response.WriteAsync("Hello from 1st middleware.");
            //});
            //app.Run(async (context) =>
            //{
            //    await context
            //    .Response.WriteAsync("Hello from 2nd middleware.");
            //});
            //app.Use(async (context,next) => 
            //{
            //    await context
            //    .Response.WriteAsync("Hello from 1st middleware.");
            //    await next();
            //});
            //app.Run(async (context) =>
            //{
            //    await context
            //    .Response.WriteAsync("Hello from 2nd middleware.");
            //});

            //taking middleware after 'UseStaticFiles' to execute static files like css,javascript first
            app.UseStaticFiles();
            //use mvc middleware
            //app.UseMvcWithDefaultRoute();
            //commenting UseMvcWithDefaultRoute use UseMvc method for conventional routing
            //? mark to make id optional
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //Commenting above conventional routing and using attribute routing 
            //For that we have to use below middleware
            //app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context
            //    .Response.WriteAsync(env.EnvironmentName);
            //}); 
        }
    }
}
