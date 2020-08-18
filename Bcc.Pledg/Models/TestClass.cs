using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bcc.Pledg.Models.Branch;

namespace Bcc.Pledg.Models
{
    public class TestClass
    {
        public TestClass()
        {
            sectors = new Sectors[0];
        }
        public string city { get; set; }
        public Sectors[] sectors { get; set; }
    }
}
