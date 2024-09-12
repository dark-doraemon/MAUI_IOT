using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Implements
{
    public interface IAuthService
    {
        public Task<bool> IsAuthenticatedAsync();

        public void Login();
        public void Logout();

    }
}
