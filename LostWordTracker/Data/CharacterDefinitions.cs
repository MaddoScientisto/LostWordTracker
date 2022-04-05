using System.Collections.Generic;

namespace LostWordTracker.Data
{
    public class CharacterDefinitions
    {
        public Dictionary<int, CharacterDefinition> Characters { get; set; }

        public IList<CharacterStorage> CharacterStorage { get; set; }
    }
}
