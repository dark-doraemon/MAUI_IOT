using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models
{
    public class Lesson
    {
        public string LessonId { get; set; }
        public string LessonName { get; set; }
        public string LessonDescription { get; set; }
        public string LessonStatus { get; set; }
        public int LessonPoint {  get; set; }
        public string LessonImage { get; set; } = "https://images.theconversation.com/files/191827/original/file-20171025-25516-g7rtyl.jpg?ixlib=rb-4.1.0&rect=0%2C70%2C7875%2C5667&q=45&auto=format&w=926&fit=clip";

        public ADXL345Sensor Sensor { get; set; }
    }
}
