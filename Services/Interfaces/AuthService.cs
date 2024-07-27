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
        public async Task<bool> IsAuthenticatedAsync()
        {
            Task.Delay(2000);
            return false;
        }
    }
}
