using MAUI_IOT.Services.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Interfaces
{
    class AuthService : IAuthService
    {
        private const string AuthStateKey = "AuthState";
        public async Task<bool> IsAuthenticatedAsync()
        {
            Task.Delay(2000);

            var authState = Preferences
                .Default.Get<bool>(AuthStateKey, false);

            return authState;
        }

        public void Login()
        {
            Preferences.Default.Set<bool>(AuthStateKey, true);
        }
        public void Logout()
        {
            Preferences.Default.Remove(AuthStateKey);
        }
    }
}
