using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreCache.Models
{
    public class KoreCacheKey
    {
        public long BucketId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
