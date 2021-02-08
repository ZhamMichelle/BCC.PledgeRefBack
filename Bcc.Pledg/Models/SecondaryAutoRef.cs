using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models
{
    public class SecondaryAutoRef
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public int? ProduceYear { get; set; }
        public long? MarketCost { get; set; }
        public int? MaxPercentageDeviation { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
