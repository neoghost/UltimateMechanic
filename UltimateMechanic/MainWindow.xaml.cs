using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UltimateMechanic.ViewModels;

namespace UltimateMechanic;

/// <summary>
/// Main window for the Ultimate Mechanic application.
/// Serves as the container for the dashboard and cleaner views.
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// </summary>
    /// <param name="viewModel">The main view model for managing navigation and state.</param>
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _viewModel = viewModel;
        Loaded += MainWindow_Loaded;
    }

    /// <summary>
    /// Handles the window loaded event and loads initial system information on startup.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The routed event arguments.</param>
    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Load dashboard on startup
        await _viewModel.DashboardViewModel.LoadSystemInfoCommand.ExecuteAsync(null);
    }
}