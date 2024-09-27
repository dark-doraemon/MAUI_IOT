using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class ExperimentManager
    {
        [PrimaryKey]
        public string ExperimentManagerId { get; set; } = string.Empty;
        public string ExperimentManagerName { get; set; } = string.Empty;
    }
}
