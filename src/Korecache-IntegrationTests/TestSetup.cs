using KoreCache.Models;
using KoreCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Korecache_IntegrationTests
{
    public static class TestSetup
    {
        public static string GetApiKey()
        {
            var apiKey = Environment.GetEnvironmentVariable("KoreCacheApiKey");
            if(!string.IsNullOrWhiteSpace(apiKey))
            {
               return apiKey;
            }
            else
            {
                using (var stream = new StreamReader("env.local"))
                {
                    var content = stream.ReadToEnd();
                    foreach(var line in content.Split(Environment.NewLine))
                    {
                        var firstEqual = line.IndexOf('=');
                        if (line.StartsWith("KoreCacheApiKey"))
                        {
                            return line.Substring(firstEqual + 1);
                        }
                    }
                }
            }

            throw new InvalidDataException($@"Invalid API key configuration for integration testing");
        }
    }
}
