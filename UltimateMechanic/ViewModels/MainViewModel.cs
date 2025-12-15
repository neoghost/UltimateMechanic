using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UltimateMechanic.ViewModels
{
    /// <summary>
    /// Main view model that manages navigation between different views in the application.
    /// Controls the current active view (Dashboard or Cleaner) and provides navigation commands.
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        /// <summary>Gets or sets the application title.</summary>
        [ObservableProperty]
        private string _title = "Ultimate Mechanic";

        /// <summary>Gets or sets the currently displayed view object.</summary>
        [ObservableProperty]
        private object? _currentView;

        /// <summary>Gets or sets the name of the current page being displayed.</summary>
        [ObservableProperty]
        private string _currentPage = "Dashboard";

        /// <summary>Gets the dashboard view model instance.</summary>
        public DashboardViewModel DashboardViewModel { get; }

        /// <summary>Gets the cleaner view model instance.</summary>
        public CleanerViewModel CleanerViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// <param name="dashboardViewModel">The dashboard view model.</param>
        /// <param name="cleanerViewModel">The cleaner view model.</param>
        public MainViewModel(DashboardViewModel dashboardViewModel, CleanerViewModel cleanerViewModel)
        {
            DashboardViewModel = dashboardViewModel;
            CleanerViewModel = cleanerViewModel;
            
            // Set initial view
            CurrentView = DashboardViewModel;
        }

        /// <summary>
        /// Navigates to the dashboard view and loads system information.
        /// </summary>
        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentView = DashboardViewModel;
            CurrentPage = "Dashboard";
            _ = DashboardViewModel.LoadSystemInfoCommand.ExecuteAsync(null);
        }

        /// <summary>
        /// Navigates to the cleaner view.
        /// </summary>
        [RelayCommand]
        private void NavigateToCleaner()
        {
            CurrentView = CleanerViewModel;
            CurrentPage = "Cleaner";
        }
    }
}
