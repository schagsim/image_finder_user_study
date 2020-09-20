using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageFinderUserStudyWeb.Services;
using ImageFinderUserStudyWeb.Services.SorterServices;
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
            var configService = new GlobalConfigService();
            var globalConfig = configService.LoadConfig(Configuration);
            var userSessionsManager = new UserSessionsManager();
            
            var labelsService = new LabelSorterService();
            var histogramService = new HistogramSorterService();

            var imageLabelsPath = Configuration["FilePaths:ImageLabelsPath"];
            var colorHistogramsPath = Configuration["FilePaths:ColorHistogramFiles"];

            var imageLabelsWrapper = new LoadedImageLabelsWrapper(labelsService.ParseLabels(imageLabelsPath));
            var histogramsWrapper = new LoadedHistogramsWrapper(histogramService.ParseHistograms(colorHistogramsPath));

            services.AddSingleton(globalConfig);
            services.AddSingleton(configService);
            services.AddSingleton(userSessionsManager);
            services.AddSingleton(labelsService);
            services.AddSingleton(histogramService);
            services.AddSingleton(imageLabelsWrapper);
            services.AddSingleton(histogramsWrapper);
            
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