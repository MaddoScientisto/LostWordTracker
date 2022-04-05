using LostWordTracker.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostWordTracker.Services
{
    public interface IDataService
    {
        Task<IList<CharacterDefinition>> LoadCharacterDefinitions();
        Task<IList<Character>> GetCharactersData();
        Task SaveData(IList<Character> characters);
        Task<IList<Character>> LoadData();
    }
}
