# RimWorld Mod Template

A .NET template for RimWorld mods prioritizing **clean builds, automation, and sane deployment**.

Stop bloating players' workshop folders with megabytes of useless files in `.git/`, `.vs/`, or `obj/`. This template helps you develop mods professionally with a robust MSBuild pipeline that handles bundling, packaging, and deployment automatically. It prevents the "Ate without table" of modding: uploading your entire source tree to Steam.

## Installation

1. Clone this repository.
2. Install the template via CLI:

    ```powershell
    dotnet new install .
    ```

3. **Visual Studio**: Restart VS. You will now see "RimWorld Mod Template" in the "Create a new project" list.

## The Pipeline (CI/CD Ready)

The standout feature is the **embedded build pipeline** in [RimWorldModTemplate.csproj](./RimWorldModTemplate.csproj). It hooks directly into standard MSBuild targets, eliminating the need for manual commands.

| Target            | Triggered By | Description                                                                       |
| ----------------- | ------------ | --------------------------------------------------------------------------------- |
| **Distribute**    | Build        | Compiles, bundles dependencies (ILRepack), and stages a clean release in `Dist/`. |
| **Deploy**        | Build        | Symlinks `Dist/` to your RimWorld `Mods/` folder.                                 |
| **PrepareLaunch** | Build        | Generates `ModsConfig.xml` and a launch script for immediate debugging.           |
| **CustomClean**   | Clean        | Wipes generated artifacts and links.                                              |

## Quick Start

1.  **Create**: Select the template in Visual Studio.
    *   The wizard will prompt you for `Mod Author`, `Mod ID`, and `Game Directory`.
    *   *Tip: Hover over the "i" icon next to fields for descriptions.*
2.  **Build**:

    ```powershell
    dotnet build
    ```
    *   **Automatically** compiles and bundles dependencies.
    *   **Automatically** distributes to `Dist/`.
    *   **Automatically** deploys (links) to your RimWorld `Mods/` folder.
    *   **Automatically** generates `ModsConfig.xml` and `LaunchRimWorld.bat`.
3.  **Run**:
    *   Execute `LaunchRimWorld.bat` (generated in `bin\Debug` or `bin\Release`).
    *   This launches the game using a **local SaveData folder** (`SaveData/` in your project root). This ensures your main game configs and saves are never touched by your dev environment.

## Key Features

*   **Zero Junk**: `Dist/` contains only what the game needs. No `.git`, `.vs`, or `obj`.
*   **Auto-About**: `About.xml` is generated from project properties.
*   **Bundled Deps**: Harmony and RimWorld refs included via NuGet.
*   **Isolated Environment**: Uses a local `SaveData` folder to keep your actual game settings clean.

## Distribution Layout

Unlike most mods, this template encourages wrapping shared content (Textures, Sounds, Defs) in a `Shared` subfolder within `Dist/`.

This structure prevents unexpected conflicts when some content is designed to load conditionally. The `Public/` folder in your source mirrors this structure directly to `Dist/`, so organize your source assets accordingly. The `Assemblies` folder is automatically populated with your compiled DLLs during the build.

## Debugging Setup (Advanced)

Thanks to [CuteLasty](https://ludeon.com/forums/index.php?action=profile;u=107532), we can set up a proper debugging environment. The steps below outline [his tutorial](https://ludeon.com/forums/index.php?topic=51589.0) from the Ludeon Forums. These steps are not automated in this template as they involve installing the Unity Editor and modifying game files.

1.  **Get Unity**: Check the Unity version of your RimWorld install (Properties of `UnityPlayer.dll`). Download the matching **Unity Editor** from the [Unity Archive](https://unity.com/releases/editor/archive) and install it.
2.  **Prepare Game Copy**: Copy your RimWorld installation to a new folder (e.g., `RimWorld_Debug`). You can use your main install, but a separate copy is safer.
3.  **Modify Game Copy**: Navigate to your Unity installation folder. Copy `UnityPlayer.dll`, `WinPixEventRuntime.dll`, and `WindowsPlayer.exe` from `Editor\Data\PlaybackEngines\windowsstandalonesupport\Variations\win64_player_development_mono` to your debug game copy, replacing existing files (`WindowsPlayer.exe` replaces `RimWorldWin64.exe`). If using your main install, back up these files first.
4.  **Enable Debugging**: Create `RimWorldWin64_Data\boot.config` in the debug folder and add:
    ```text
    wait-for-managed-debugger=1
    player-connection-debug=1
    ```
5.  **Visual Studio**: Install the **Visual Studio Tools for Unity** workload.
6.  **Attach**: Update the `GameDir` variable in your `.csproj` to point to your debug game copy. Build and run. A dialog will appear, prompting you to attach an debugger; go to `Debug -> Attach Unity Debugger` in VS and select the process.
7.  **Debug**: Enjoy breakpoints, variable inspection, and all the usual debugging features!