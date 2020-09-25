using Bcc.Pledg.Models.Branch;

namespace Bcc.Pledg.Models
{
    public class SectorsCityTester
    {
        public SectorsCityTester()
        {
            type = "";
            city = "";
            sectors = new Sectors[0];
        }
        public string type { get; set; }
        public string city { get; set; }
        public Sectors[] sectors { get; set; }
    }
}
