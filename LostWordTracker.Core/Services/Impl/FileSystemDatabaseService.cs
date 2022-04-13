using LostWordTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services.Impl
{
    public class FileSystemDatabaseService : IDatabaseService
    {
        private readonly IConfigurationService _config;
        public FileSystemDatabaseService(IConfigurationService configurationService)
        {
            _config = configurationService;
        }

        public async Task<CharacterDefinitions> LoadCharacterDefinitions()
        {
            var fileText = await File.ReadAllTextAsync(_config.DataPath);
            var deserializedObject = JsonSerializer.Deserialize<CharacterDefinitions>(fileText);

            return deserializedObject;
        }
    }
}
