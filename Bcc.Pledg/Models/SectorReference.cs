using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models
{
    public class SectorReference
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string SectorCode { get; set; }
        public string Sector { get; set; }
        public List<Coordinates> Coordinates { get; set; }
    }
}
