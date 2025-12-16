# 🔧 Ultimate Mechanic

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![Platform](https://img.shields.io/badge/platform-windows-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)

**Ultimate Mechanic** is a modern, lightweight system maintenance tool for Windows built with **WPF** and **.NET 9**. It helps you monitor system resources, clean junk files, and manage startup programs to keep your PC running smoothly.

---

## Features included:

### System Dashboard
Real-time monitoring of your system's vital statistics:
* **CPU Usage:** Live processor tracking.
* **Memory Status:** RAM usage visualization.
* **Storage Health:** Capacity and usage bars for all fixed drives.

### Smart Cleaner
Intelligent scanning engine that finds and removes:
* **Windows Junk:** Temp files, Prefetch, Event Logs, Error Reports.
* **Browser Caches:** Chrome, Edge, Firefox.
* **System Cache:** Thumbnail cache, Recycle Bin.
* *Safety First:* "Scan" before you "Clean" with granular selection.

### Startup Manager
Take control of your Windows boot time:
* **View All:** Lists programs from both Registry and Startup Folders.
* **Toggle:** Enable or disable apps with a single click.
* **Delete:** Permanently remove unwanted entries (with Trash Can safety check).

---

## Screenshots

| **Startup Manager** | **System Cleaner** |
|:---:|:---:|
| ![Startup Manager](docs/images/startup_view.png) | ![Cleaner View](docs/images/cleaner_view.png) |

> *Note: Dark Mode is enabled by default for a slick, modern look.*

---

## Tech Stack

* **Framework:** .NET 9.0 (Windows)
* **UI:** WPF (Windows Presentation Foundation)
* **Architecture:** MVVM (Model-View-ViewModel)
* **Libraries:**
    * `CommunityToolkit.Mvvm` (Messaging, Commands)
    * `Microsoft.Extensions.DependencyInjection` (IoC Container)
    * `System.Management` (WMI for Hardware Info)

---

## Getting Started

### Prerequisites
* Windows 10 or 11
* .NET 9.0 Runtime

### Installation
1.  Clone the repository:
    ```bash
    git clone [https://github.com/neoghost/UltimateMechanic.git](https://github.com/neoghost/UltimateMechanic.git)
    ```
2.  Open `UltimateMechanic.sln` in Visual Studio.
3.  Build and Run:
    ```bash
    dotnet build
    dotnet run --project UltimateMechanic/UltimateMechanic.csproj
    ```

---

## License

Distributed under the MIT License. See `LICENSE` for more information.

---
*Built by Alon Kerem*
