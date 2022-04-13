using LostWordTracker.Core.Services;
using LostWordTracker.Core.Services.Impl;
using LostWordTracker.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace LostWordTracker.Test
{
    public class Tests
    {
        private readonly IServiceProvider _services;

        [SetUp]
        public void Setup()
        {
            var provider = Provider();
            //var serv = provider.GetRequiredService<IDataService>();
        }

        public static IServiceProvider Provider()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDataService, DummyDataService>();
            services.AddScoped<IGenericLocalStorageService, MemoryStorageService>();

            return services.BuildServiceProvider();
        }

        [Test]
        public void Test1()
        {


            
        }
    }
}