using LostWordTracker.Data;
using LostWordTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services.Impl
{
    public class DummyDataService : IDataService
    {
        public string ExportCompressed(CharacterDefinitions characters)
        {
            throw new NotImplementedException();
        }

        public string ExportRaw(CharacterDefinitions characters)
        {
            throw new NotImplementedException();
        }

        public Task<CharacterDefinitions> GetCharactersData()
        {
            throw new NotImplementedException();
        }

        public Task<CharacterDefinitions> ImportCompressed(string data)
        {
            throw new NotImplementedException();
        }

        public Task<CharacterDefinitions> ImportRaw(string data)
        {
            throw new NotImplementedException();
        }

        public Task<CharacterDefinitions> LoadCharacterDefinitions()
        {
            throw new NotImplementedException();
        }

        public Task<CharacterDefinitions> LoadData()
        {
            throw new NotImplementedException();
        }

        public Task SaveData(CharacterDefinitions characters)
        {
            throw new NotImplementedException();
        }
    }
}
