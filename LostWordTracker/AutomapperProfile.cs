using AutoMapper;
using LostWordTracker.Data;

namespace LostWordTracker
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CharacterStorage, Character>();
            CreateMap<Character, CharacterStorage>();
            CreateMap<CharacterDefinition, Character>();            
        }
    }
}
