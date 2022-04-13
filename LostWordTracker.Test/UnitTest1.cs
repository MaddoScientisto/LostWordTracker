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
using System.Linq;

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
        public async Task LoadCharacterDefinitions()
        {

            var dataService = _services.GetRequiredService<IDataService>();

            //var mockedData = new Mock<IDataService>

            var characters = await dataService.LoadCharacterDefinitions();

            characters.Should().NotBeNull();

            characters.Characters.Should().NotBeNullOrEmpty().And.ContainKeys(new int[] { 1, 2 });

            characters.CharacterStorage.Should().BeNull();

        }

        [Test]
        public async Task LoadCharacterData()
        {

            var dataService = _services.GetRequiredService<IDataService>();

            var characters = await dataService.GetCharactersData();

            characters.Should().NotBeNull();

            characters.Characters.Should().NotBeNullOrEmpty().And.ContainKeys(new int[] { 1, 2 });

            characters.CharacterStorage.Should().NotBeNullOrEmpty().And.ContainEquivalentOf(new CharacterStorage()
            {
                Id = 1,
                Level = 0,
                Obtained = false,
                LimitBreak = 0
            });


        }

        [Test]
        public async Task SaveLoadData()
        {
            var dataService = _services.GetRequiredService<IDataService>();

            var dataToSave = new System.Collections.Generic.List<CharacterStorage>()
                {
                     new CharacterStorage()
            {
                Id = 1,
                Level = 100,
                LimitBreak = 5,
                Obtained = true
            }
        };




            var testChars = new CharacterDefinitions()
            {
                //Characters = new System.Collections.Generic.Dictionary<int, CharacterDefinition>
                //{
                //    {1, new CharacterDefinition() }
                //},
                CharacterStorage = dataToSave
            };

            await dataService.SaveData(testChars);

            var data = await dataService.LoadData();

            data.Characters.Should().NotBeNullOrEmpty().And.ContainKeys(new int[] { 1, 2 });
            data.CharacterStorage.Should().NotBeNullOrEmpty().And.ContainEquivalentOf(dataToSave.First());
        }

        [Test]
        public async Task ImportExportData()
        {
            var dataService = _services.GetRequiredService<IDataService>();

            string dataString = "[{\"Id\":1,\"Obtained\":true,\"Level\":100,\"LimitBreak\":5},{\"Id\":2,\"Obtained\":false,\"Level\":0,\"LimitBreak\":0}]";
            var data = await dataService.Import(dataString);

            data.Should().NotBeNull();

            data.CharacterStorage.Should().NotBeNullOrEmpty().And.ContainEquivalentOf(new CharacterStorage()
            {
                Id = 1,
                Level = 100,
                LimitBreak = 5,
                Obtained = true
            }).And.ContainEquivalentOf(new CharacterStorage()
            {
                Id = 2,
                Obtained = false,
                Level = 0,
                LimitBreak = 0
            });

            var exportedData = dataService.Export(data);
            exportedData.Should().NotBeNull().And.BeEquivalentTo(dataString);
        }

   
    }
}