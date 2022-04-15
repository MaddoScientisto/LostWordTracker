using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services
{
    public interface IDrawingService
    {
        Task<byte[]> MakeImage(bool drawLevel, bool drawSkills);
    }
}
