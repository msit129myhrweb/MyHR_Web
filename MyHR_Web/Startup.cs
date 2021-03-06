using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHR_Web.Common;
using MyHR_Web.Hubs;
using MyHR_Web.IService;
using MyHR_Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //services.AddSession();
            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.IdleTimeout = TimeSpan.FromHours(1);
                //options.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential = true;
            });

            services.AddRazorPages()   //[Required] Attribute對於「DateTime」、「int」等實值型別沒有作用： (!!所以不能使用自定義錯誤訊息)
            .AddMvcOptions(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "此欄位不得為空");

            });
            services.AddSignalR();
            Global.ConnectionString = Configuration.GetConnectionString("dbMyCompany");
            services.AddScoped<INotiService, NotiService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");       
            });
        }
    }
}
