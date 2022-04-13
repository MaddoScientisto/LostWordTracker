using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services.Impl
{
    public class ConfigurationService : IConfigurationService
    {
        public string DataPath { get { return "Characters.json"; }  }
    }
}
