using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class Config
    {
        public string Name { get; set; }
        public int PacketNumber { get; set; }
        public DataConfig DataConfig { get; set; }
        public Config(int samplingDuration, int samplingRate)
        {
            Name = "SET_CONFIG";
            PacketNumber = 50;
            DataConfig = new DataConfig()
            {
                SamplingDuration = samplingDuration,
                SamplingRate = samplingRate
            };
        }
    }

    public class DataConfig
    {
        public int SamplingDuration { get; set; }
        public int SamplingRate { get; set; }
    }
}
