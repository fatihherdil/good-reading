using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;

namespace GoodReading.Web.Api.Integration.Tests
{
    public class TestFixture : IDisposable
    {
        protected IHost Host { get; set; }
        protected IConfiguration Configuration { get; set; }
        protected IGoodReadingContext GoodReadingContext { get; set; }

        public HttpClient HttpClient { get; protected set; }
        
        public TestFixture()
        {
            SetConfiguration();
            Host = CreateHostBuilder().StartAsync().Result;
            GoodReadingContext = Host.Services.GetService(typeof(IGoodReadingContext)) as IGoodReadingContext;
            HttpClient = Host.GetTestClient();
        }

        private void SetConfiguration()
        {
            var path = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(path, "appsettings.Development.json");

            Configuration = new ConfigurationBuilder().AddJsonFile(configPath).Build();
        }

        private IHostBuilder CreateHostBuilder()
        {
            var builder = new HostBuilder()
                .UseSerilog((context, services, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext();
                })
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseConfiguration(Configuration)
                        .UseTestServer();
                });

            return builder;
        }

        public void Dispose()
        {
            Host?.Dispose();
            HttpClient?.Dispose();

            #region Delete Everything From MongoDB

            GoodReadingContext.Products.DeleteMany(Builders<Domain.Entities.Product>.Filter.Empty);
            GoodReadingContext.Customers.DeleteMany(Builders<Domain.Entities.Customer>.Filter.Empty);
            GoodReadingContext.CustomerOrders.DeleteMany(Builders<Domain.Entities.CustomerOrder>.Filter.Empty);

            #endregion
        }
    }
}
