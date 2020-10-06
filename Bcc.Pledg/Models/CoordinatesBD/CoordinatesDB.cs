using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models.CoordinatesBD
{
    public class CoordinatesDB
    {
        public int Id { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public string SectorsDBId { get; set; }
        public SectorsDB SectorsDB { get; set; }
    }
}
