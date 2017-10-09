using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingMetricsGrafana
{
    public class RootObject
    {
        public string database { get; set; }
        public List<Machine> machines { get; set; }
    }
}
