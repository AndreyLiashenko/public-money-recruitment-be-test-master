using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using VacationRental.DAL.Core;
using Xunit;

namespace VacationRental.Api.Tests
{
    [CollectionDefinition("Integration")]
    public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public IntegrationFixture()
        {
            _server = new TestServer(
                new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services => 
                {
                    string connectionString = "Database=VacationRental;Server=.\\SQLEXPRESS;Integrated Security=True;";
                    services.AddDbContext<VacationRentalContext>(options => options.UseSqlServer(connectionString));
                    
                }));

            Client = _server.CreateClient();

        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
