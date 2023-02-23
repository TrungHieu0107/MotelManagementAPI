using BussinessObject.Data;
using DataAccess.Repository;
using DataAccess.Service;
using DataAccess.Service.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MotelManagementAPI.BackgroundService.ScheduledTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagementAPI
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

            services.AddControllers();
            services.AddDbContext<Context>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IElectricityCostService, ElectricityCostService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IMotelChainService, MotelChainService>();
            services.AddScoped<IResidentService, ResidentService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IWaterCostService, WaterCostService>();

            services.AddScoped<IAccountRepo, AccountRepo>();
            services.AddScoped<IElectricityCostRepo, ElectricityCostRepo>();
            services.AddScoped<IHistoryRepo, HistoryRepo>();
            services.AddScoped<IInvoiceRepo, InvoiceRepo>();
            services.AddScoped<IManagerRepo, ManagerRepo>();
            services.AddScoped<IMotelChainRepo, MotelChainRepo>();
            services.AddScoped<IResidentRepo, ResidentRepo>();
            services.AddScoped<IRoomRepo, RoomRepo>();
            services.AddScoped<IWaterCostRepo, WaterCostRepo>();

            services.AddSingleton<IHostedService, AutoCheckLateInvoices>();
            services.AddSingleton<IHostedService, AutoCloseAndCreateInvoices>();
            services.AddSingleton<IHostedService, AutoUpdateBookedRoomsToActive>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MotelManagementAPI", Version = "v1" });
            });

            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                  .AllowAnyHeader());
            });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotelManagementAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
