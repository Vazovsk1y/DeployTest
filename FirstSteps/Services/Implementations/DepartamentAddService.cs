using FirstSteps.View.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSteps.Services.Implementations
{
    internal class DepartamentAddService : IUserDialog
    {
        private IServiceProvider _serviceProvider;
        private MainWindow? _mainWindow;
        private DepartamentAdd? _departamentAddWindow;

        public DepartamentAddService(IServiceProvider serviceProvider)
        { 
            _serviceProvider = serviceProvider;
        }

        public void CloseSecondaryWindow()
        {
            if (_departamentAddWindow is { } window)
            {
                window.Close();
                return;
            }

            window = _serviceProvider.GetRequiredService<DepartamentAdd>();
            window.Closed += (_, _) => _departamentAddWindow = null;

            _departamentAddWindow = window;
            window.Close();
        }

        public void OpenMainWindow()
        {
            if (_mainWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _serviceProvider.GetRequiredService<MainWindow>();
            window.Closed += (_, _) => _mainWindow = null;

            _mainWindow = window;
            window.ShowDialog();
        }

        public void OpenSecondaryWindow()
        {
            if(_departamentAddWindow is { } window )
            {
                window.ShowDialog();
                return;
            }

            window = _serviceProvider.GetRequiredService<DepartamentAdd>();
            window.Closed += (_, _) => _departamentAddWindow = null;

            _departamentAddWindow = window;
            window.ShowDialog();
        }

        public Task OpenSecondaryWindowAsync()
        {
            if (_departamentAddWindow is { } window)
            {
                window.ShowDialog();
                return Task.CompletedTask;
            }

            window = _serviceProvider.GetRequiredService<DepartamentAdd>();
            window.Closed += (_, _) => _departamentAddWindow = null;

            _departamentAddWindow = window;
            window.ShowDialog();
            return Task.CompletedTask;
        }
    }
}
