using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class Experiment
    {
        [PrimaryKey]
        public string ExperimentId { get; set; }
        [Unique]
        public string ExperimentName { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(ExperimentManager))]
        public string ExperimentManagerId { get; set; } = string.Empty;
    }
}
