using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_IOT.Models;
using MAUI_IOT.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly HomeViewModel vm;
        [ObservableProperty]
        private Account _account;

        public LoginViewModel(HomeViewModel vm)
        {
            _account = new Account();
            this.vm = vm;
        }

        [RelayCommand]
        public async void Login()
        {
            Shell.Current.GoToAsync($"//{nameof(HomeView)}");
            //App.Current.MainPage = new NavigationPage(new HomeView(vm));  
        }
    }
}
