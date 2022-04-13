using LostWordTracker.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostWordTracker.Services
{
    public interface IDataService
    {
        Task<CharacterDefinitions> LoadCharacterDefinitions();
        Task<CharacterDefinitions> GetCharactersData();
        Task SaveData(CharacterDefinitions characters);
        Task<CharacterDefinitions> LoadData();

        Task<CharacterDefinitions> Import(string data);

        string Export(CharacterDefinitions characters);
    }
}
