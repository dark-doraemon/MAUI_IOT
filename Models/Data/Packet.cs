using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class Packet
    {
        public string name { get; set; }
        public int packetNumber { get; set; }
        public List<Data> data { get; set; }
    }
}
