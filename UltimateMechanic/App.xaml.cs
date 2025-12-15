using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using UltimateMechanic.ViewModels;
using UltimateMechanic.Services;

namespace UltimateMechanic
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Services
            services.AddSingleton<ISystemInfoService, SystemInfoService>();
            services.AddSingleton<ICleanerService, CleanerService>();
            services.AddSingleton<IStartupService, StartupService>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<CleanerViewModel>();
            services.AddSingleton<StartupViewModel>();

            // Views
            services.AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = Services.GetRequiredService<MainWindow>();
            
            // CRITICAL FIX: Tell WPF that this is the Main Window
            Current.MainWindow = mainWindow;
            
            mainWindow.Show();
        }
    }
}
