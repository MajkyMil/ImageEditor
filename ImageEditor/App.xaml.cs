using ImageEditor.View;
using ImageEditor.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImageEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            SetupExceptionHandling();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show(e.ExceptionObject.ToString(), $"AppDomain.CurrentDomain.UnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), $"AppDomain.CurrentDomain.DispatcherUnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), $"AppDomain.CurrentDomain.UnobservedTaskException", MessageBoxButton.OK, MessageBoxImage.Error);
            };

        }
    }
}
