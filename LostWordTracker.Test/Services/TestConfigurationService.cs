using LostWordTracker.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Test.Services
{
    public class TestConfigurationService : IConfigurationService
    {
        public string DataPath { get { return "testdata.json"; } }
    }
}
