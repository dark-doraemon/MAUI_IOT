﻿using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class Data
    {
        [PrimaryKey, AutoIncrement]
        public int DataId { get; set; }
        public int timestamp {get; set;}
        public double accX { get; set; }
        public double accY { get; set; }
        public double accZ { get; set; }
        public double force { get; set; }

        [ForeignKey(nameof(ExperimentInfo))]
        public int ExperimentInfoId { get; set; }
    }
}