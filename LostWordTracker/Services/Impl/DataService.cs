using AutoMapper;
using LostWordTracker.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LostWordTracker.Services.Impl
{
    public class DataService : IDataService
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
        private readonly IMapper _mapper;
        public DataService(HttpClient httpClient, Blazored.LocalStorage.ILocalStorageService localStorage, IMapper mapper)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _mapper = mapper;
        }

        public async Task<IList<CharacterDefinition>> LoadCharacterDefinitions()
        {
            return (await _httpClient.GetFromJsonAsync<CharacterDefinitionContainer>("Characters.json")).Characters;
        }

        public async Task<IList<Character>> GetCharactersData()
        {
            var loadedCharacters = await LoadCharacterDefinitions();
            var characters = _mapper.Map<IList<Character>>(loadedCharacters);
            return characters;
        }

        public async Task SaveData(IList<Character> characters)
        {

            var characterStorageContainer = new CharacterStorageContainer()
            {
                Characters = _mapper.Map<IList<CharacterStorage>>(characters)
            };

            await _localStorage.SetItemAsync<CharacterStorageContainer>("CharacterData", characterStorageContainer);
        }

        public async Task<IList<Character>> LoadData()
        {
            var loadedData = await _localStorage.GetItemAsync<CharacterStorageContainer>("CharacterData");
            IList<CharacterStorage> characterStoredData = loadedData.Characters;  // source two
            IList<CharacterDefinition> charactersData = await LoadCharacterDefinitions(); // source one

            var characters = _mapper.Map<IList<Character>>(charactersData);

            // hack
            int i = 0;
            foreach(var character in characters)
            {
                character.Obtained = characterStoredData[i].Obtained;
                character.Level = characterStoredData[i].Level;
                i++;
            }
            //_mapper.Map<IList<CharacterStorage>,IList<Character>>(characterStoredData, characters);

            return characters;
        }
    }
}
