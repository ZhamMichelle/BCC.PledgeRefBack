using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models
{
    public class WallMaterialReference
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int? WallMaterialCodeColvir { get; set; }
        public string WallMaterialColvir { get; set; }
        public int? WallMaterialCodeGF { get; set; }
        public string WallMaterialGF { get; set; }
    }
}
