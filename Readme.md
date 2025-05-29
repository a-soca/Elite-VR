![Elite VR Logo](/Assets/Images/Readme/Elite-VR-Logo-Horizontal.png)

# Credits
This project was made possible by Mark Moxon, David Braben, Ian Bell, Steve Bagley, and all contributors of [BeebEm](https://github.com/stardot/beebem-windows/graphs/contributors).

Non original model attributions can be found [here](/Documentation/Asset-Attribution.md).

For those interested in exploring the source code of Elite, you can find a wealth of information on Mark Moxon's site [here](https://elite.bbcelite.com/).

# Compilation Guides
## Emulator (BeebEm)
The original source code for BeebEm is available at https://github.com/stardot/beebem-windows.

If you want to compile BeebEm yourself then you will need Microsoft Visual Studio 2019 or later (the free VS2019 Community edition will compile BeebEm). The following project files are included:

| File                                 | Description                       |
| ------------------------------------ | --------------------------------- |
| `BeebEm.sln`                         | Solution file                     |
| `BeebEm.vcxproj`                     | BeebEm project file               |
| `Hardware\Watford\Acorn1770.vcxproj` | Acorn 1770 FDC project file       |
| `Hardware\Watford\OpusDDOS.vcxproj`  | Opus DDOS FDC project file        |
| `Hardware\Watford\Watford.vcxproj`   | Watford FDC project file          |
| `InnoSetup\Installer.vcxproj`        | Inno Setup installer project file |
| `ZipFile\ZipFile.vcxproj`            | Distribution zip project file     |

Please note that these project files are set up to target Windows XP, which we use to create release binaries. This requires the following optional Visual Studio 2019 components to be installed:

* MSVC v140 - VS 2015 C++ build tools (v14.00)
* C++ Windows XP Support for VS 2017 (v141) tools [Deprecated]

To build for Windows XP you will also need to download and install the [Microsoft DirectX 9.0 SDK (June 2010)](https://www.microsoft.com/en-us/download/details.aspx?id=6812). If you are building for Windows 10 or later only, you don't need to do this. Instead ensure the following Visual Studio 2019 components are installed:

* MSVC v142 - VS 2019 C++ X64/X86 build tools
* C++ MFC for latest v142 build tools (x86 & x64)

and see [Instructions for people who only need Windows 10 compatibility](#instructions-for-people-who-only-need-windows-10-compatibility).

To build the installer from within Visual Studio, you'll need to download and install [Inno Setup 5.6.1](https://files.jrsoftware.org/is/5/).

To build the distribution BeebEm.zip from within Visual Studio, you'll need to install Perl, e.g., [Strawberry Perl](https://strawberryperl.com/).

### Configuration

After installing the DirectX 9.0 SDK and Inno Setup, the next step is to configure the BeebEm Visual Studio project to find the relevant files.

Rename the file `Src\BeebEm.user.props.example` to `Src\BeebEm.user.props`, and then open `BeebEm.sln` in Visual Studio.

Select the **View** menu, then **Other Windows**, then **Property Manager**. In the **Property Manager** window, click to expand **BeebEm\Release | Win32** and then double-click on **BeebEm.user**.

This opens the BeebEm.user properties. Select **User Macros** from the list in the left column, under **Common Properties**, then set the following macro values:

* Set `DXSDK_Dir` to the path to the DirectX SDK, e.g:

  Name:  `DXSDK_Dir`
  Value: `C:\Program Files\Microsoft DirectX SDK (June 2010)`

* Set `ISCC_Dir` to the path to the Inno Setup compiler, e.g:

  Name:  `ISCC_Dir`
  Value: `C:\Program Files\Inno Setup 5`

The **Set this macro as an environment variable in the build environment** option does not need to be ticked.

### Instructions for people who only need Windows 10 compatibility

If you don't mind targeting Windows 10, you may attempt the following unsupported steps. The advantage of doing this is that you don't need to install anything apart from Visual Studio 2019; the disadvantage is that the EXE will only support Windows 10, and the installer and distribution aren't supported.

Firstly, ensure `C++ MFC for latest v142 build tools (x86 & x64)` is installed.

1. Allow Visual Studio to retarget projects to v141_xp
2. Get properties for the BeebEm project, and select **All Configurations** and **All Platforms**
3. In **General**, set **Platform Toolset** to `Visual Studio 2019 (v142)`, and click OK
4. Get properties for the BeebEm project, and select **All Configurations** and **All Platforms**
5. In **General**, set **Windows SDK Version** to **10.0 (latest installed version)**, and click OK

Now build.

### Other Operating Systems

This version of BeebEm will not compile on Unix systems. This may change at some point but for now if you want to run BeebEm on Unix please download a Unix specific version of BeebEm.