using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class DataSummarize
    {
        [PrimaryKey, AutoIncrement]
        public long DataSummarizeId { get; set; }
        public double Std_A { get; set; }
        public double Std_F { get; set; }
        public double AvgA { get; set; }
        public double AvgF { get; set; }

        [ForeignKey(nameof(ExperimentConfig))]
        public string ExperimentId { get; set; } = string.Empty;
    }
}
