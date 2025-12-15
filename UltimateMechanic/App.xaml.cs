using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using UltimateMechanic.ViewModels;
using UltimateMechanic.Services;

namespace UltimateMechanic
{
    /// <summary>
    /// Application entry point for Ultimate Mechanic.
    /// Configures and manages dependency injection for all services, view models, and views.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the current application instance with access to its services.
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the service provider for resolving registered services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Initializes a new instance of the App class and configures dependency injection.
        /// </summary>
        public App()
        {
            Services = ConfigureServices();
        }

        /// <summary>
        /// Configures all services, view models, and views for dependency injection.
        /// </summary>
        /// <returns>A configured IServiceProvider instance.</returns>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Services
            services.AddSingleton<ISystemInfoService, SystemInfoService>();
            services.AddSingleton<ICleanerService, CleanerService>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<CleanerViewModel>();

            // Views
            services.AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Handles application startup by showing the main window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The startup event arguments.</param>
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
