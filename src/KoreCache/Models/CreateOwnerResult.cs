using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreCache.Models
{
    public class CreateOwnerResult
    {
        public User NewUser { get; set; }
        public ApiKey InitialApiKey { get; set; }
    }
}
