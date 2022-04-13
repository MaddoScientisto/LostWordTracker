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
using LostWordTracker.Test.Services;

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
            services.AddSingleton<IGenericLocalStorageService, MemoryStorageService>();

            services.AddScoped<IDatabaseService, FileSystemDatabaseService>();                       
            services.AddAutoMapper(typeof(AutomapperProfile));

            services.AddSingleton<IConfigurationService, TestConfigurationService>();

            return services.BuildServiceProvider();
        }

        [Test]
        public async Task LoadCharacterDefinitions()
        {

            var dataService = _services.GetRequiredService<IDataService>();

            var characters = await dataService.LoadCharacterDefinitions();

            characters.Should().NotBeNull();

            characters.Characters.Should().NotBeNullOrEmpty().And.ContainKeys(new int[] { 1, 2 });

            var exampleCharacter = new CharacterDefinition()
            {
                Id = 1,
                Name = "Reimu Hakurei",
                Universe = "L1",
                Portrait = "100101_Reimu_Hakurei_SQ.webp",
                Tier = "S",
                TierRank = 0,
                FarmTier = "A",
                CqTier = "S",
                ObtainType = "Prayer",
                Enabled = true,
            };

            characters.Characters[1].Should().BeEquivalentTo(exampleCharacter);

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
        public async Task ImportExportDataRaw()
        {
            var dataService = _services.GetRequiredService<IDataService>();

            string dataString = "[{\"Id\":1,\"Obtained\":true,\"Level\":100,\"LimitBreak\":5},{\"Id\":2,\"Obtained\":false,\"Level\":0,\"LimitBreak\":0}]";
            var data = await dataService.ImportRaw(dataString);

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

            var exportedData = dataService.ExportRaw(data);
            exportedData.Should().NotBeNull().And.BeEquivalentTo(dataString);
        }

        [Test]
        public async Task ExportDataCompressed()
        {
            var dataService = _services.GetRequiredService<IDataService>();
            var data = await dataService.GetCharactersData();
            data.CharacterStorage[1].Obtained = true;
            data.CharacterStorage[1].Level = 100;
            data.CharacterStorage[1].LimitBreak = 5;

            var exportedData = dataService.ExportCompressed(data);

            exportedData.Should().NotBeNullOrWhiteSpace().And.BeEquivalentTo("aAAAAB+LCAAAAAAAAAqKrlbyTFGyMtRR8k8qSczMSwVy0hJzilN1lHxSy1JzlKwMgKzM3MwSp6LUxGwgt1YHosUIWUtJUSlCh6EBmh7T2lgAAAAA//8=");
        }

        [Test]
        public async Task ImportExportDataCompressed()
        {
            var dataService = _services.GetRequiredService<IDataService>();

            string dataString = "aAAAAB+LCAAAAAAAAAqKrlbyTFGyMtRR8k8qSczMSwVy0hJzilN1lHxSy1JzlKwMgKzM3MwSp6LUxGwgt1YHosUIWUtJUSlCh6EBmh7T2lgAAAAA//8=";
            var data = await dataService.ImportCompressed(dataString);

            data.Should().NotBeNull();

            data.CharacterStorage.Should().NotBeNullOrEmpty().And.ContainEquivalentOf(new CharacterStorage()
            {
                Id = 2,
                Level = 100,
                LimitBreak = 5,
                Obtained = true
            });

            var exportedData = dataService.ExportCompressed(data);
            exportedData.Should().NotBeNull().And.BeEquivalentTo(dataString);
        }

        [Test]
        public async Task CharacterAddedToMasterList()
        {
            //var baseModel = new CharacterDefinitions()
            //{
            //    Characters = new System.Collections.Generic.Dictionary<int, CharacterDefinition> {
            //        { 1, new CharacterDefinition()
            //        {
            //            Id = 1,
            //            Name = "Reimu Hakurei",
            //            Enabled = true,
            //            CqTier = "S",
            //            FarmTier = "S",
            //            ObtainType = "Pray",
            //            Portrait = "test.webp",
            //            Tier = "S",
            //            TierRank = 0,
            //            Universe = "L1"
            //        }
            //        },
            //        {2, new CharacterDefinition()
            //        {
            //            Id = 2,
            //            Name = "Marisa Kirisame",
            //            Enabled = true,
            //            CqTier = "B",
            //            FarmTier = "B",
            //            ObtainType = "Pray",
            //            Portrait = "test2.webp",
            //            Tier = "B",
            //            TierRank = 0,
            //            Universe = "L1"
            //        }
            //        }
            //    }

            //};

          
            var dataService = _services.GetRequiredService<IDataService>();
          
            var characters = await dataService.GetCharactersData();
            var lastObject = characters.CharacterStorage.Last();
            characters.CharacterStorage.Remove(lastObject);

            characters.CharacterStorage.Should().HaveCount(1);

            await dataService.SaveData(characters);

            //characters = await dataService.GetCharactersData();

            //characters.Characters.Should().HaveCount(2);

            //characters.CharacterStorage.Should().HaveCount(2);

            characters = await dataService.LoadData();

            characters.Characters.Should().HaveCount(2);

            characters.CharacterStorage.Should().HaveCount(2);
        }

    }
}