using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImageFinderUserStudyWeb
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
            int.TryParse(Configuration["GallerySettings:GalleryWidthPixels"], out var galleryWidth);
            int.TryParse(Configuration["GallerySettings:GalleryHeightPixels"], out var galleryHeight);
            // TODO: The scroll bars in the gallery can be different. Is there a good way how to solve it?
            //    The problem with the added pixel width is that if we don't add the additional pixels, the table
            //    is not going to fit the table wrapper and a side scrollbar in the bottom appears.
            int.TryParse(Configuration["GallerySettings:GalleryScrollBarPixels"], out var scrollBarPixels);
            var globalConfig = new GlobalConfig(
                galleryWidth,
                galleryHeight,
                scrollBarPixels
            );
            var userSessionsManager = new UserSessionsManager();

            services.AddSingleton(globalConfig);
            services.AddSingleton(userSessionsManager);
            
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}