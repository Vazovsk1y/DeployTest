using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSteps.Services
{
    public interface IUserDialog
    {
        void OpenSecondaryWindow();
        void OpenMainWindow();
        void CloseSecondaryWindow();
        Task OpenSecondaryWindowAsync();
    }
}
