﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CustomerManagement.Data;
using Microsoft.EntityFrameworkCore;


namespace CustomerManagement
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<CustomerDbContext>(options =>
            {
                options.UseSqlServer(sqlConnectionString);
            });


            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IStatesRepository, StatesRepository>();
            services.AddTransient<DbSeeder>();



            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbSeeder dbSeeder)
        {
            app.UseDeveloperExceptionPage();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            dbSeeder.SeedAsync().Wait();
            app.UseMvc();
        }
    }
}//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CustomerManagement.Data;
//using CustomerManagement.Models;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;

//namespace CustomerManagement
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            string sqlConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
//            services.AddDbContext<CustomerDbContext>(options =>
//            {
//                options.UseSqlServer(sqlConnectionString);
//            });


//            services.AddScoped<ICustomersRepository, CustomersRepository>();
//            services.AddScoped<IStatesRepository, StatesRepository>();
//            services.AddTransient<DbSeeder>(); //SEED İÇİN
//            services.AddMvc();
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory, DbSeeder dbSeed)
//        {
//            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
//            loggerFactory.AddDebug();
//            dbSeed.SeedAsync().Wait();
//            app.UseMvc();
//        }
//    }
//}
