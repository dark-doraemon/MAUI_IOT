using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_IOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private Account _account;

        public LoginViewModel()
        {
            _account = new Account();
        }

        [RelayCommand]
        public void Login()
        {
            string username = _account.Username;
            string password = _account.Password;
        }
    }
}
