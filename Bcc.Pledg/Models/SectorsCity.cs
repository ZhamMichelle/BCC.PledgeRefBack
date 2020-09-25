using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bcc.Pledg.Models.Branch;

namespace Bcc.Pledg.Models
{
    public class SectorsCity
    {
        public SectorsCity()
        {
            type = "";
            city = "";
            sectors = new List<Sectors>();
        }
        public string type { get; set; }
        public string city { get; set; }
        public List<Sectors> sectors { get; set; }
    }


}
