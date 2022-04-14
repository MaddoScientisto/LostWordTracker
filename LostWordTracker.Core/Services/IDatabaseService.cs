using LostWordTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services
{
    public interface IDatabaseService
    {
        Task<CharacterDefinitions> LoadCharacterDefinitions();
        Task<Byte[]> GetFont(string path);
    }
}
