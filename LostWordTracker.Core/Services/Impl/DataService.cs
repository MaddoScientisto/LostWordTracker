using AutoMapper;
using LostWordTracker.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System;
using LostWordTracker.Core.Services;

namespace LostWordTracker.Services.Impl
{
    public class DataService : IDataService
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly IGenericLocalStorageService _localStorage;
        private readonly IMapper _mapper;
        public DataService(HttpClient httpClient, IGenericLocalStorageService localStorage, IMapper mapper)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _mapper = mapper;
        }

        public async Task<CharacterDefinitions> LoadCharacterDefinitions()
        {
            return await _httpClient.GetFromJsonAsync<CharacterDefinitions>("Characters.json");
        }

        public async Task<CharacterDefinitions> GetCharactersData()
        {
            var loadedCharacters = await LoadCharacterDefinitions();
            loadedCharacters.CharacterStorage = new List<CharacterStorage>();
            foreach (var character in loadedCharacters.Characters)
            {
                loadedCharacters.CharacterStorage.Add(new CharacterStorage()
                {
                    Id = character.Key,
                    Level = 0,
                    Obtained = false,
                    LimitBreak = 0
                });
            }
            
            return loadedCharacters;
        }

        public async Task SaveData(CharacterDefinitions characters)
        {            
            await _localStorage.SetItemAsync<CharacterStorageContainer>("CharacterData", new CharacterStorageContainer() { Characters = characters.CharacterStorage });
        }

        public async Task<CharacterDefinitions> LoadData()
        {
            var loadedData = await _localStorage.GetItemAsync<CharacterStorageContainer>("CharacterData");
            
            var charactersData = await LoadCharacterDefinitions();
            charactersData.CharacterStorage = new List<CharacterStorage>();

            foreach (var character in charactersData.Characters)
            {
                var loadedChar = loadedData.Characters.FirstOrDefault(x => x.Id == character.Key);
                if (loadedChar == null) continue;
                charactersData.CharacterStorage.Add(new CharacterStorage()
                {
                    Id = character.Key,
                    Level = loadedChar.Level,
                    Obtained = loadedChar.Obtained,
                    LimitBreak = loadedChar.LimitBreak
                });
            }

           

            return charactersData;
        }

        public async Task<CharacterDefinitions> Import(string data)
        {
            //var loadedData = JsonSerializer.Deserialize<IList<CharacterStorage>>(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String( data)));

            var loadedData = JsonSerializer.Deserialize<IList<CharacterStorage>>(data);

            var charactersData = await LoadCharacterDefinitions();
            charactersData.CharacterStorage = new List<CharacterStorage>();

            foreach (var character in charactersData.Characters)
            {
                var loadedChar = loadedData.FirstOrDefault(x => x.Id == character.Key);
                if (loadedChar == null) continue;
                charactersData.CharacterStorage.Add(new CharacterStorage()
                {
                    Id = character.Key,
                    Level = loadedChar.Level,
                    Obtained = loadedChar.Obtained,
                    LimitBreak = loadedChar.LimitBreak                   
                });
            }

            return charactersData;
        }

        public string Export(CharacterDefinitions characters)
        {
            string res = JsonSerializer.Serialize(characters.CharacterStorage);
            return res;
            //return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(res));

           
        }
    }
}
