using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCC.PledgeRefBack.Models
{
    public class PledgeReference
    {
        public int Id { get; set; }
        public int? CityCodeKATO { get; set; }
        public string City { get; set; }
        public string SectorCode { get; set; }
        public int? Sector { get; set; }
        public string RelativityLocation { get; set; }
        public string SectorDescription { get; set; }
        public string TypeEstateCode { get; set; }
        public string TypeEstateByRef { get; set; }
        public string TypeEstate { get; set; }
        public string ApartmentLayoutCode { get; set; }
        public string ApartmentLayout { get; set; }
        public int? WallMaterialCode { get; set; }
        public string WallMaterial { get; set; }
        public string DetailAreaCode { get; set; }
        public string DetailArea { get; set; }
        public int? MinCostPerSQM { get; set; }
        public int? MaxCostPerSQM { get; set; }
        public string CostDescription { get; set; }
    }
}
