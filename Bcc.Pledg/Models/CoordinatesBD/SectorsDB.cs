using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models.CoordinatesBD
{
    public class SectorsDB
    {
        public SectorsDB()
        {
            CoordinatesDB = new List<CoordinatesDB>();
        }
        public string Id { get; set; }       //SectorCode
        public string Sector { get; set; }
        public List<CoordinatesDB> CoordinatesDB { get; set; }
        public int SectorsCityDBId { get; set; }
        public SectorsCityDB SectorsCityDB { get; set; }
    }
}
