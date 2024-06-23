using AutoShop.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Testcontainers.MsSql;
using Xunit;

namespace AutoShop.Tests
{
    public class IntegretionTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .Build();

        private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:latest")
            .Build();

        //private readonly RedisContainer _redisContainer
        //= new RedisBuilder().Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var msqlDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (msqlDescriptor != null)
                {
                    services.Remove(msqlDescriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(_msSqlContainer.GetConnectionString());
                });

                var serviceProvider = services.BuildServiceProvider();
                using var scopeMsql = serviceProvider.CreateScope();
                var db = scopeMsql.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();

                var mongoDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(IMongoDatabase));

                if (mongoDescriptor != null)
                {
                    services.Remove(mongoDescriptor);
                }

                services.AddSingleton<IMongoClient>(sp =>
                {
                    return new MongoClient(_mongoDbContainer.GetConnectionString());
                });

                services.AddScoped<IMongoDatabase>(sp =>
                {
                    var mongoClient = sp.GetRequiredService<IMongoClient>();
                    return mongoClient.GetDatabase("autoshop");
                });

                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });
            });
        }


        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();
            await _mongoDbContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
            await _mongoDbContainer.StopAsync();
        }
    }
}
