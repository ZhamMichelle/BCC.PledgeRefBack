using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bcc.Pledg.Models
{
    

    public class Coordinates
    {
        [JsonProperty("lat")]
        public double lat { get; set; }  //y

        [JsonProperty("lng")]
        public double lng { get; set; }  //x
    }
}
