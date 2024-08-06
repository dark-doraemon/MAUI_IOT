using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_IOT.Models;
using MAUI_IOT.Services.Interfaces;
using MAUI_IOT.Views;
using Microsoft.Maui.Controls.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ILessionService lessionService;
        [ObservableProperty]
        ObservableCollection<Lesson> lessons;

        public HomeViewModel(ILessionService lessionService)
        {
            this.lessionService = lessionService;
            lessons =  lessionService.GetLessons();
        }

        [RelayCommand]
        async Task MoveToLesson(string LessonId)
        {
            var lesson = lessionService.GetLessonById(LessonId);
            Dictionary<string, object> paramaters = new Dictionary<string, object>
            {
                {"data",lesson }
            };
            await Shell.Current.GoToAsync($"{nameof(LessonView)}", paramaters);
        }
    }
}
