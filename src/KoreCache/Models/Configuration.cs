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

        public Configuration(string apiKey)
        {
            ApiKey = apiKey;
        }
    }
}
