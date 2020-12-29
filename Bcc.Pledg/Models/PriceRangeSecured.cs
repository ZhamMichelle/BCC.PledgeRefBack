using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models
{
    public class PriceRangeSecured
    {
        public char TypeCode { get; set; }
        public string NameStrategy { get; set; }
        public string CityCodeKATO { get; set; }
        public string Sector { get; set; }
        public string TypeEstateCode { get; set; }
        public string ApartmentLayoutCode { get; set; }
        public int? WallMaterialCode { get; set; }
        public string DetailAreaCode { get; set; }
    }
}
