using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreCache.Models
{
    public class Configuration
    {
        public string ApiKey { get; private set; }
        public string KoreToolsEnvironment { get; private set; }

        public Configuration(string apiKey, string koreToolsEnvironment = "https://prod.us-west-1.kore-tools.com")
        {
            ApiKey = apiKey;
            KoreToolsEnvironment = koreToolsEnvironment;

        }
    }
}
