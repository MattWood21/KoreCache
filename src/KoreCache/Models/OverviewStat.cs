using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreCache.Models
{
    public class OverviewStat
    {
        public string Name { get; set; }
        public string Stat { get; set; }
        public string Max { get; set; }
        public string Percentage { get; set; }
        public int PercentageValue { get; set; }
    }
}
