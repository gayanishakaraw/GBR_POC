using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBR.Entity
{
    public class Tenant
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DbConnectionString { get; set; }
        public string Status { get; set; }
    }
}
