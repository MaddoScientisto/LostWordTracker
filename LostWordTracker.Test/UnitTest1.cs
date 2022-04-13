using LostWordTracker.Core.Services;
using LostWordTracker.Core.Services.Impl;
using LostWordTracker.Services;
using LostWordTracker.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;
using LostWordTracker.Data;
using Moq.Protected;
using System.Threading;
using System.Net;
using System.IO;
using FluentAssertions;

namespace LostWordTracker.Test
{
    public class Tests
    {
        private IServiceProvider _services;



        [SetUp]
        public void Setup()
        {
            _services = Provider();
            //var serv = provider.GetRequiredService<IDataService>();
        }

        public static IServiceProvider Provider()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IGenericLocalStorageService, MemoryStorageService>();



            var exampleData = new StringContent(File.ReadAllText("testdata.json"));

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = exampleData,
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            services.AddSingleton<HttpClient>(httpClient);

            services.AddAutoMapper(typeof(AutomapperProfile));

            return services.BuildServiceProvider();
        }

        [Test]
        public async Task LoadCharacters()
        {

            var dataService = _services.GetRequiredService<IDataService>();

            //var mockedData = new Mock<IDataService>

            var characters = await dataService.LoadCharacterDefinitions();
            
            characters.Should().NotBeNull();

            characters.Characters.Should().NotBeNull().And.ContainKeys(new int[] { 1, 2 });


        }
    }
}