using System;
using System.Windows;
using FirstSteps.Services;
using FirstSteps.Services.Implementations;
using FirstSteps.VievModel;
using FirstSteps.View.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace FirstSteps
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IServiceProvider? _services;

        public static IServiceProvider Services => _services ??= InitializeServices().BuildServiceProvider();

        private static ServiceCollection InitializeServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainWindowViewModel>();
            services.AddScoped<DepartamentAddService>(); // i add 
            services.AddSingleton<DepartamentAddViewModel>();

            services.AddSingleton<IUserDialog, DepartamentAddService>();
            services.AddSingleton<IMessageBus, MessageBusService>();

            services.AddTransient(
                s =>
                {
                    var model = s.GetRequiredService<MainWindowViewModel>();
                    var window = new MainWindow { DataContext = model };
                    return window;
                });

            services.AddTransient(
                s =>
                {
                    var scope = s.CreateScope();
                    var model = scope.ServiceProvider.GetRequiredService<DepartamentAddViewModel>();
                    var window = new DepartamentAdd { DataContext = model };
                    window.Closed += (_, _) => scope.Dispose();

                    return window;
                });


            return services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);  
            Services.GetRequiredService<DepartamentAddService>().OpenMainWindow();
        }
    }
}
