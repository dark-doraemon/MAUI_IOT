﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models
{
    public class Axis
    {
        public DateTime TimeStamp { get; set; } 
        public float x {  get; set; }
        public float y {  get; set; }
        public float z {  get; set; }
    }
}
