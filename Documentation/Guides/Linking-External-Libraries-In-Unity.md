# Linking External Libraries in Unity
- Guide for linking external libraries in Unity

There are several ways to introduce external libraries into Unity, and which one to use depends on the format (.dll, .cs, NuGet package, etc.) and the source of the external library. Below is a detailed description of all the common methods of introduction.

| Index                                                                                  |
| -------------------------------------------------------------------------------------- |
| [Importing dlls](#Method-1-Manually-Importing-DLL-Dynamic-Link-Library)                |
| [Importing C# Source](#method-2-direct-import-of-cs-source-code)                       |
| [Importing with UPM](#method-3-importing-via-gitnuget-using-unity-package-manager-upm) |

## Method 1: Manually Importing .DLL (Dynamic Link Library)

### Step 1: Create plug-in folder
- In a Unity project, you need to create a plugin folder for external library files, this helps to organise and manage the external library files.
- In the Unity compiler, click on the Assets folder in the project panel and select "Create" -> "Folder" to create a new folder and name it "Plugins" (or whatever is appropriate).

![Create Folder](/Assets/Images/Linking-External-Libraries-In-Unity/createFolder.png)

### Step 2: Place the external library file into the plug-in folder
- Copy the .dll file to the Plugins directory.
- For libraries that depend on more than one file (e.g. Newtonsoft.Json may contain more than one `.dll`), make sure that all relevant `.dll` files are in the same directory.
- If the library has source code (`.cs` file), you can also put it in the Plugins directory or Assets/Scripts/ and Unity will compile it automatically.

### Step 3: Ensure Unity recognises the DLL
- Check the import settings of the .dll in the Unity Inspector panel:
1. Select the `.dll` file in the Plugins folder.
2. In the Inspector panel, find Select platforms for plugin and select the applicable platform (e.g. Windows, Android, iOS).
3. If the library contains a Native Plugin (`.dll` written in C++), tick the Load on Startup option.

### Step 4: Use of external libraries in code
- Add using statements to the C# code file (`.cs`), for example:

![Code Example 1](/Assets/Images/Linking-External-Libraries-In-Unity/codeExample1.png)

## Method 2: Direct import of .CS source code

### Step 1: Download or copy the .CS file
- If the external library is pure C# source code (e.g. Utility.cs), you can download the `.cs` file directly.
- For example, [Json.NET](https://github.com/applejag/Newtonsoft.Json-for-Unity) for Unity provides a pure C# version.

### Step 2: Put .CS file into Unity
- Create a folder in the `Assets/Scripts/` or `Assets/Plugins/` directory, for example `ExternalLibs`.
- Copy the `.cs` file directly to the directory.

### Step 3: Reference in code
- Directly use in the C# file:

![Code Example 2](/Assets/Images/Linking-External-Libraries-In-Unity/codeExample2.png)

## Method 3: Importing via Git/NuGet Using Unity Package Manager (UPM)

### Step 1: Open Unity Package Manager
- In Unity, click `Window > Package Manager`
- Click the + sign in the top left corner and select "Add package from git URL..."..".
- Choose your method of installing the package

![Unity Package Manager](/Assets/Images/Linking-External-Libraries-In-Unity/packageManager.png)

For example, enter the URL of the GitHub repository
https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git

- Click Add and Unity will automatically download and install the library.

### Step 2: Reference in code

![Code Example 3](/Assets/Images/Linking-External-Libraries-In-Unity/codeExample3.png)