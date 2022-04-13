using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services
{
    public interface IConfigurationService
    {
        string DataPath { get; }
    }
}
