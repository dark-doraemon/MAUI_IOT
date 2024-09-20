using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models
{
    public class ExperimentInfo
    {
        public string Device {  get; set; } = string.Empty;
        public double Weight { get; set; }
        public int SamplingRate { get; set;}
        public int SamplingDuration { get; set;}
    }
}
