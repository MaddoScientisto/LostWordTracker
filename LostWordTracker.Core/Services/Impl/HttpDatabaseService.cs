using AutoMapper;
using LostWordTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services.Impl
{
    public class HttpDatabaseService : IDatabaseService
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IConfigurationService _configurationService;

        public HttpDatabaseService(HttpClient httpClient, IMapper mapper, IConfigurationService configurationService)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _configurationService = configurationService;
        }

        public async Task<CharacterDefinitions> LoadCharacterDefinitions()
        {
            return await _httpClient.GetFromJsonAsync<CharacterDefinitions>(_configurationService.DataPath);
        }
    }
}
