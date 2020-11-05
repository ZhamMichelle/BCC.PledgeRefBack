using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models
{
    public class PrimaryPledgeRef
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CityCodeKATO { get; set; }
        public string City { get; set; }
        public int? RCNameCode { get; set; }
        public string RCName { get; set; }
        public string ActualAdress { get; set; }
        public string FinQualityLevelCode { get; set; }
        public string FinQualityLevel  { get; set; }
        public int? MinCostPerSQM { get; set; }
        public int? MaxCostPerSQM { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
