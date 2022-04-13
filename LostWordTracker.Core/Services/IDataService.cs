using LostWordTracker.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostWordTracker.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Loads the character definitions but without user data
        /// </summary>
        /// <returns></returns>
        Task<CharacterDefinitions> LoadCharacterDefinitions();

        /// <summary>
        /// Loads both definitions and user data
        /// </summary>
        /// <returns></returns>
        Task<CharacterDefinitions> GetCharactersData();
        Task SaveData(CharacterDefinitions characters);
        Task<CharacterDefinitions> LoadData();

        Task<CharacterDefinitions> ImportCompressed(string data);
        Task<CharacterDefinitions> ImportRaw(string data);

        string ExportRaw(CharacterDefinitions characters);
        string ExportCompressed(CharacterDefinitions characters);
    }
}
