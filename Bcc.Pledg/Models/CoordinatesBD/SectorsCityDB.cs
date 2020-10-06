using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models.CoordinatesBD
{
    public class SectorsCityDB
    {
        public SectorsCityDB()
        {
            SectorsDB = new List<SectorsDB>();
        }
        public int Id { get; set; }
        public string Type { get; set; }
        public string City { get; set; }
        public List<SectorsDB> SectorsDB { get; set; }
    }
}
