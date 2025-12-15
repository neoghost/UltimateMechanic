# GitHub Copilot Instructions for UltimateMechanic

This document outlines essential information for AI coding agents working on the `UltimateMechanic` codebase.

## 1. Project Overview & Architecture
- **Purpose:** `UltimateMechanic` is a Windows-only desktop application (WPF) for system cleanup and information display.
- **Architecture:** Follows the Model-View-ViewModel (MVVM) pattern.
    - **Models (`Models/`):** Define data structures (e.g., `SystemInfo`, `CleanupItem`, `CleanupGroup`).
    - **Views (`Views/`):** XAML files defining the user interface (e.g., `DashboardView.xaml`, `CleanerView.xaml`).
    - **ViewModels (`ViewModels/`):** C# classes that expose data from Models to Views and handle UI logic (e.g., `DashboardViewModel`, `CleanerViewModel`). They use `CommunityToolkit.Mvvm` for observable properties and commands.
    - **Services (`Services/`):** C# classes encapsulating business logic and data access (e.g., `SystemInfoService` for system hardware info, `CleanerService` for scanning/cleaning). These services often interact with Windows-specific APIs (WMI, Performance Counters).
    - **Converters (`Converters/`):** WPF value converters for transforming data between ViewModel and View (e.g., `ValueConverters.cs` for `BoolToVisibilityConverter`).

## 2. Key Workflows & Commands
- **Build:**
  - The primary build command is `dotnet build`.
  - To clean and rebuild: `dotnet clean && dotnet build`.
  - The project is configured for `net9.0-windows`.
- **Run:**
  - To run the application: `dotnet run --project UltimateMechanic.csproj`.
- **Test:**
  - Test project: `Tests/UltimateMechanic.Tests/UltimateMechanic.Tests.csproj` (MSTest framework).
  - To run all tests: `dotnet test`.
  - If tests are not discovered, ensure the `Tests/UltimateMechanic.Tests/UnitTests/ModelTests.cs` file exists and contains `[TestClass]` and `[TestMethod]` attributes.
- **Solution File:** `UltimateMechanic.sln` includes the main application and test projects.

## 3. Project-Specific Conventions & Patterns
- **Windows-Only:** The application heavily relies on Windows-specific APIs (`System.Management`, `PerformanceCounter`). Ensure any new features or modifications are compatible with Windows.
- **MVVM Implementation:** `CommunityToolkit.Mvvm` is used for `ObservableProperty` attributes and `RelayCommand` for UI commands.
- **Cleaner Grouping:** The `CleanerViewModel` groups `CleanupItem`s into `CleanupGroup`s for display. Logic for selection synchronization and total size calculation resides in `CleanerViewModel` and `CleanupGroup`.
- **Dashboard Auto-Refresh:** The `DashboardViewModel` uses a `DispatcherTimer` to periodically refresh system information.

## 4. Important Files & Directories
- `App.xaml` / `App.xaml.cs`: Application entry point and resource dictionary (e.g., `BoolToVisibilityConverter`).
- `UltimateMechanic.csproj`: Main application project file, targeting `net9.0-windows`.
- `Tests/UltimateMechanic.Tests/UltimateMechanic.Tests.csproj`: Test project file.
- `Models/CleanupGroup.cs`: Defines the grouping logic for cleanup items.
- `ViewModels/CleanerViewModel.cs`: Core logic for the Cleaner feature, including scanning, cleaning, and grouping.
- `ViewModels/DashboardViewModel.cs`: Core logic for the Dashboard feature, including system info loading and auto-refresh.
- `Services/SystemInfoService.cs`: Interacts with Windows APIs to gather system information.
- `Services/CleanerService.cs`: Handles file system scanning and cleanup operations.

Please review and provide feedback if any sections are unclear, incomplete, or require further detail.
