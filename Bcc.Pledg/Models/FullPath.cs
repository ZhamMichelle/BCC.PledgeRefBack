using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Models
{
    public class FullPath
    {
        public string fullPath { get; set; }
    }

    public class City { 
        public string city { get; set; }
    }

    public class DeleteParams { 
        public string City { get; set; }
        public int? Sector { get; set; } 
    }
}
