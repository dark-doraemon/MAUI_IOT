using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class Data
    {
        public TimeSpan timetamp { get; set; }
        public double accX { get; set; }
        public double accY { get; set; }
        public double accZ { get; set; }
        public double force { get; set; }
    }
}