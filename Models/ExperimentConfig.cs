using CommunityToolkit.Mvvm.ComponentModel;
using MAUI_IOT.Models.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models
{
    public class ExperimentConfig
    {
        [PrimaryKey, AutoIncrement]
        public int ExperimentConfigId { get; set; }
        public string Device {  get; set; } = string.Empty;
        public double Weight { get; set; }
        public int SamplingRate { get; set;}
        public int SamplingDuration { get; set;}

        [ForeignKey(nameof(Experiment))]
        public string ExperimentId { get; set; } = string.Empty;
    }
}
