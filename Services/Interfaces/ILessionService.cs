using MAUI_IOT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Interfaces
{
    public interface ILessionService
    {
        ObservableCollection<Lesson> GetLessons();

        Lesson GetLessonById(string id);
    }
}
