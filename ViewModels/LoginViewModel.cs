using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_IOT.Models;
using MAUI_IOT.Services.Implements;
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
        private readonly IAuthService authService;
        [ObservableProperty]
        private Account _account;

        public LoginViewModel(HomeViewModel vm,IAuthService authService)
        {
            _account = new Account();
            this.vm = vm;
            this.authService = authService;
        }

        public LoginViewModel()
        {
            
        }

        [RelayCommand]
        public async void Login()
        {
            //authService.Login();
            Shell.Current.GoToAsync($"//{nameof(HomeView)}");
            //App.Current.MainPage = new NavigationPage(new HomeView(vm));  
        }
    }
}
