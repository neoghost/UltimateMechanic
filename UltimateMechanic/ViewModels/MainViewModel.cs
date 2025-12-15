using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;

namespace UltimateMechanic.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "Ultimate Mechanic";

        [ObservableProperty]
        private object? _currentView;

        [ObservableProperty]
        private string _currentPage = "Dashboard";

        // Expose the real version to the UI
        public string AppVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

        public DashboardViewModel DashboardViewModel { get; }
        public CleanerViewModel CleanerViewModel { get; }
        public StartupViewModel StartupViewModel { get; }

        public MainViewModel(
            DashboardViewModel dashboardViewModel, 
            CleanerViewModel cleanerViewModel,
            StartupViewModel startupViewModel)
        {
            DashboardViewModel = dashboardViewModel;
            CleanerViewModel = cleanerViewModel;
            StartupViewModel = startupViewModel;
            
            CurrentView = DashboardViewModel;
        }

        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentView = DashboardViewModel;
            CurrentPage = "Dashboard";
            _ = DashboardViewModel.LoadSystemInfoCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private void NavigateToCleaner()
        {
            CurrentView = CleanerViewModel;
            CurrentPage = "Cleaner";
        }

        [RelayCommand]
        private void NavigateToStartup()
        {
            CurrentView = StartupViewModel;
            CurrentPage = "Startup";
            _ = StartupViewModel.LoadStartupItemsCommand.ExecuteAsync(null);
        }
    }
}
