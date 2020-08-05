using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models.Branch
{
    public class Sectors
    {
        public Sectors()
        {
            coordinates = new CoordinatesXY[0];
        }
        public int sector { get; set; }
        public string sectorCode { get; set; }
        public CoordinatesXY[] coordinates { get; set; }
    }




    public class CoordinatesXY
    {
        public double lng { get; set; }
        public double lat { get; set; }
    }
}
