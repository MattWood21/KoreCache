using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreCache.Models
{
    public class ApiKey
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
    }
}
