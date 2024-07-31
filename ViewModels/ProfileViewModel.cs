using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_IOT.Services.Implements;
using MAUI_IOT.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAuthService authService;

        public ProfileViewModel(IAuthService authService)
        {
            this.authService = authService;
        }
        [RelayCommand]
        async Task Logout()
        {
            authService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }
    }

}
