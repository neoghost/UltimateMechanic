using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
            
            // Set initial view
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
            // Auto-load items when navigating to the page
            _ = StartupViewModel.LoadStartupItemsCommand.ExecuteAsync(null);
        }
    }
}
