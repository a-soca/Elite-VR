# Elite VR Software Manual

| Index                                                                  |
| ---------------------------------------------------------------------- |
| [High Level Architecture](#high-level-architecture)                    |
| [Emulator](#emulator)                                                  |
| [Compiling the Emulator](#compiling-the-elite-vr-custom-beebem-fork)   |
| [Running the Emulator (Standalone)](#running-the-elite-vr-beebem-fork) |
| [Custom Emulator Debug Commands](#custom-debug-commands)               |
| [Custom Emulator Configuration](#custom-configuration)                 |
| [Using Debug Commands to Load Elite](#use-debug-command-to-load-game)  |
| [Controlling the Emulator](#controlling-the-emulator)                  |
| [Extracting Data from the Emulator Memory](#extracting-data)           |
| [Writing Data to the Emulator Memory](#writing-data)                   |
| [Simulating Emulator Input](#simulating-input)                         |
| [Sending Data from the Emulator](#sending-data)                        |
| [The Transport Layer](#transport-layer)                                |
| [Unity Project](#unity)                                                |
| [Running the Game In Editor](#running-the-game-in-editor)              |
| [Building the Unity project](#building-elite-vr)                       |
| [Running Elite VR](#running-elite-vr)                                  |
| [Emulator Display Passthrough](#emulator-display-passthrough)          |
| [Scenes](#scenes)                                                      |
| [Scene Transitions](#scene-transitions)                                |
| [Utility Scripts](#other-utility-scripts)                              |
| [Miscellaneous Notes](#miscellaneous-notes)                            |



## High Level Architecture
There are 3 core components handled within Elite VR: 
-	Emulation 
-	Transport
-	Presentation 

![High Level Architecture](/Assets/Images/Software-Manual/EliteVR.png)

> Note: The VR Interface is abstracted away via the use of OpenXR and OVR within Unity, handling controller inputs and hand tracking. With that being said, each additional button must be manually mapped in the unity input manager as a custom interaction to be accessible in scripts (a guide can be found [here](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html)).

The emulation component handles running the game, simulating input, reading and writing to memory, and hosting the server. The transport layer is a UDP server-client interface utilising a fork of the ENet library for compatibility. The presentation component is a Unity project housing the client with scripts for translating the received data and using said data to perform desired actions on game objects (for example updating positions).

> Note: To provide a simple interface for developers to access data of both the emulator and the game, the façade design pattern is utilised. To manage behaviour of a large quantity of objects from a single or small group of scripts in the presentation component, the mediator pattern is utilised. These patterns should be respected and followed in future feature implementations to enforce consistency.

## Emulator
The Emulator of choice for Elite VR is BeebEm. The source code for this emulator can be found [here](https://github.com/stardot/beebem-windows), alongside documentation specific to the base code.

### BeebEm Source Code
### Elite VR Custom BeebEm Fork
The Elite VR Project uses a custom fork of BeebEm with added classes and modified functionality. The relevant new classes/files and their dependencies are shown in the diagram below:

![Emulator Class Relationships](/Assets/Images/Software-Manual/EmulatorRelationships.png)

> Note: Main, BeebWin and Debug are files native to the source code, however they contain modifications specific to this project. EliteFacade, MemoryAccessor, InputSimulator and EmulatorController are Facades created to abstract away specific implementations of operations.

#### Compiling the Elite VR Custom BeebEm Fork
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

##### Configuration

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

##### Instructions for people who only need Windows 10 compatibility

If you don't mind targeting Windows 10, you may attempt the following unsupported steps. The advantage of doing this is that you don't need to install anything apart from Visual Studio 2019; the disadvantage is that the EXE will only support Windows 10, and the installer and distribution aren't supported.

Firstly, ensure `C++ MFC for latest v142 build tools (x86 & x64)` is installed.

1. Allow Visual Studio to retarget projects to v141_xp
2. Get properties for the BeebEm project, and select **All Configurations** and **All Platforms**
3. In **General**, set **Platform Toolset** to `Visual Studio 2019 (v142)`, and click OK
4. Get properties for the BeebEm project, and select **All Configurations** and **All Platforms**
5. In **General**, set **Windows SDK Version** to **10.0 (latest installed version)**, and click OK

Now build.

##### Other Operating Systems

This version of BeebEm will not compile on Unix systems. This may change at some point but for now if you want to run BeebEm on Unix please download a Unix specific version of BeebEm.

##### Adding Elite VR Dependencies
The `BeebEm-Release-Dependencies` folder contains files needed for the custom version of BeebEm to run.

- Place the `ENet.dll` file and `UserData` folder in the same location as `BeebEm.exe`
- Place the `elite-compendium-bbc-micro` in either the same location as `BeebEm.exe` if only running the emulator or in the same location as `StartEliteVR.bat` if testing a release with the unity build

The emulator is now ready to run
Install the necessary dependencies listed above and open the Visual Studio project in the Elite VR Emulator-Source-Code directory of the repository. You will again need to rename the `BeebEm.user.props.example` file to `BeebEm.user.props`.

#### Note: Linking Libraries
This project requires the linking of `WinMM`, `ENet`, and `ws2_32`. This can be achieved by adding `enet.lib;ws2_32.lib;WinMM.Lib;` at the start of the additional dependencies string in `Properties>Linker` if not already configured.

> Note: A full guide for linking libraries in MSVS has been included in the project repository for step by step instructions [here](/Documentation/Guides/Linking-In-MSVS.md).

#### Running the Elite VR BeebEm Fork
The result will be output to `BeebEm-Source-Code\Src\x64\Debug` or `BeebEm-Source-Code\Src\x64\Release` depending on the target. The custom version of BeebEm *MUST* have the `User Preferences` folder, `ENet.dll` and `elite-compendium-bbc-micro.dsd` present in the same folder as the executable when run for correct behaviour. These files can be found in the root directory of the repository in `BeebEm-Release-Dependencies`. Simply drag these files into the build output.

If running in conjunction with the unity project, follow the instructions in [this](#building-elite-vr) section.

### Custom Debug Commands
The BeebEm debugger has been modified to add commands specific to this project for testing during development. These can be found in the `Debug.cpp` file.

| Command                | Usage                                                                                   | Notes                                                                                              |
| ---------------------- | --------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- |
| StreamToTerminal \[x\] | StreamToTerminal 8200 will open a terminal and repeatedly print the memory address 8200 | Streams 16 bytes per refresh                                                                       |
| StreamData             | StreamData will start sending the ship block data and player data to the client         | Translates and formats raw data to reduce packet size, client workload and simplify interpretation |
| LoadElite              | LoadElite will load and run the game automatically                                      | Runs “elite-compendium-bbc-micro.dsd”                                                              |

> Note: Use of these commands is NOT necessary when the application is being run alongside Unity. They should only be used for testing the emulator on its own.

To create custom debug commands which print to a terminal window, a custom function `CreateTerminal()` has been implemented which will allow developers to simply use `std::cout` and `printf()` as normal.

### Running Elite
> Note: The targeted game version is the `Elite 6502 Co-Processor Compendium Version`

#### Custom Configuration
Before loading the game, the emulator must be configured to have a 6502 tube. To do this, a custom function `BeebWin::ApplyCustomConfiguration()` automatically injects code during the initialisation of the emulator to modify certain properties.

Additional preferences may also be applied in this function, for example setting volume and opening the debugger, if desired.

#### Use Debug Command to Load Game
Open the debugger by clicking `options>debugger` and entering the command `LoadElite`. The game will now load and run automatically.

### Controlling the Emulator
The `EmulatorController` class provides generic functions for modifying the emulator at runtime. A `LoadDisk()` function has been implemented to allow other disks to be loaded if desired alongside a `SetEmulatorSpeed()` function which can be used to increase or decrease the rate of instruction processing arbitrarily.

#### Example Usage
##### ENetServer.cpp
> ```
> EmulatorController::LoadDisk("elite-compendium-bbc-micro.dsd"); // Load elite
> ```

##### EliteStreamer.cpp
> ```
> EmulatorController::SetEmulatorSpeed(speed);
> ```

### Extracting Data
A generic interface for reading and writing to memory has been implemented to allow developers to read and write to individual bytes and blocks of memory in the `MemoryAccessor` class. This interface is found in the `MemoryAccessFacade.cpp` file. Elite specific memory access operations should be implemented in the `EliteFacade.cpp` file to maintain the generic interface for future implementations to take advantage of.

The convention for implementing memory access operations of fixed memory locations in the `EliteFacade` class is to declare a constant with the same specifier as the assembly code in the header file. This allows the value to be easily changed from version to version and makes it clear which value is being accessed within functions.

#### Example Usage
##### EliteFacade.h

> ```
> static const int FSH = 0x8F1; // Front Shield, 0-255 where 0 is empty and 255 is full
> ```

##### EliteFacade.cpp
> ```
> int EliteFacade::GetPlayerFrontShield() { // Alex Soca 24/04/2025
> 	return MemoryAccessor::GetByteFromMemory(FSH); // Front Shield status
> }
> ```

### Writing Data
Writing data works in a similar way to reading. Following the same conventions as before, declare the memory address to write to and use the `MemoryAccessor` function to write a given value to that byte.

#### Example Usage
##### EliteFacade.h
> ```
> static const int JSTY = 0x899; // Joystick Y position
> ```

##### EliteFacade.cpp
> ```
> void EliteFacade::SetPlayerPitch(int value) { // Alex Soca
>  	// .JSTY (0x899) stores the current pitch value
>	MemoryAccessor::WriteToMemory(JSTY, value);
> }
> ```

### Simulating Input

Not all game functionality can be easily achieved through writing to memory addresses. In cases where keypresses are necessary, the `InputSimulator` class in the `InputSimulationFacade.cpp` file can trigger the following:

-	Letter, Symbol, Function, and Arrow key presses
-	Letter, Symbol, and Arrow key holds
-	Letter, Symbol, and Arrow key releases

The duration of a key press (i.e. how long the key is held down for before being released) can be configured in the `KEY_PRESS_DURATION` definition in the same file.
For key presses, the argument should be passed as a VK key scan code. This can be obtained by using the `VkKeyScan()` function on a `char`.

> Note: When a hold event is triggered, developers should ensure a corresponding release event is requested after, but also before the next press/hold request of the same key.

#### Example Usage
##### ENetServer.cpp
> ```
> InputSimulator::PressKey(VkKeyScan(data[0])); // Where data[0] is the requested character
> …
> InputSimulator::PressArrowKey(data[0]); // Where data[0] is a direction [ U, D, L, R ]
> …
> int functionNumber = stoi(translated); // Convert the string to an int
> InputSimulator::PressFunctionKey(functionNumber);
> ```

### Sending Data
Once data has been extracted, it must be sent to clients to be processed by the presentation component. To do this, the EliteStreamer.cpp file handles loops for continuously extracting and sending data to the ENet Server for dispatch. Currently, there are 2 loops which handle the data of ships in the local bubble and the player’s ship information respectively.

To optimise packet size, data should be sent in a predefined format with different variables delimited with vertical bars and parameters of a variable delimited with commas. For multiple blocks of data being sent in a single packet, each block should be delimited with a newline character.

The Specification for the formats of these packets can be found [here](/Documentation/Manuals/Transport-Layer-Protocol.md/#ship-block-data-format).

Future data should be added to either one of these loops or to a new loop. For synchronous low frequency data, like checking if the player ship has been fitted with new equipment after closing the menu, a call and response method should be implemented. High frequency data like ship positions should be broadcast unreliably to reduce unnecessary traffic and processing of request packets every frame.

To organise data transmission and endpoint receipt, different channels are used for each stream. The protocol for this can be found in the Transport Layer section of this document.

To send data, use the `BroadcastString()` function provided, ensuring to use the correct channel as the second argument. Ensure the string to be broadcast has been converted to a const through the use of `c_str()` to prevent any concurrency issues.

#### Example Usage
##### EliteStreamer.cpp
```
std::string formattedData(buffer); // Create a string from the values
BroadcastStringFromServer(formattedData.c_str(), 3); // Send the player ship data on the relevant channel
```

## Transport Layer
The Elite VR transport layer is built on top of the ENet UDP library, specifically utilising the C++/C# cross compatible wrapper created by nxrightthere ([Repository Link](https://github.com/nxrighthere/ENet-CSharp)). It uses a channel-based organisation protocol to reduce packet size and routing overhead on both server and client sides. Resultantly, it is important to follow the specification listed below to prevent undefined behaviour from reading incorrectly routed data.

### ENet Server Configuration
The ENet Server is configured with 11 Channels (0-10 inclusive), listening on localhost IP `127.0.0.1` port `1337`. The Bandwidth both incoming and outgoing is `unlimited` and the buffer size has been set to `2048 Bytes`.

The Transport layer organises data routing through the use of channels, the specific protocol and channel map can be found [here](/Documentation/Manuals/Transport-Layer-Protocol.md)

## Unity
### Running the Game in Editor
To maximise iteration speed, the `Start_BeebEm` script automatically opens the custom BeebEm fork whenever the editor is run. Additionally, when the editor is stopped, the BeebEm process will be terminated.
	
Due to the architecture, it is not possible to start the game from the immersed scene as it is necessary for the intro scene to be run through to load the game in the emulator. Furthermore, components in the immersed scene depend on components which are kept alive from the intro scene, meaning they will not be present if the immersed scene is entered directly. For this reason, developers must always return to the home scene before clicking run.

### Building Elite VR
- Open Unity Hub
- Select "Open Project from Disk"
- Navigate to `Unity-Project/EliteVR` and click Open
- Create a folder somewhere safe to build the project outside of the unity project
- In the toolbar, select `File>Build Profiles`
- In the newly opened window, select windows and click `Build`
- Navigate to the folder created earlier and confirm

### Running Elite VR
- Create a new parent folder with the name format `Elite-VR-Vx.x.x`
- Add a subfolder `EliteVR`
- Copy the contents of the Unity build into this folder
- Add a subfolder `BeebEm`
- Follow the build instructions for BeebEm [Link](#building-and-running-the-elite-vr-beebem-fork), switching the build configuration in BeebEm to Release
- Copy the `Release` folder from the BeebEm Build into this folder
- Create a shortcut to both `BeebEm.exe` and `EliteVR.exe` and place them in the root of the release folder
- Place the `StartEliteVR.bat` script (found in the `BeebEm-Release-Dependencies` folder in the repo root) in the root of the release folder

![Root Directory](/Assets/Images/Software-Manual/Release-Root.png)


![Tree](/Assets/Images/Software-Manual/Release-Tree.png)

- Run the `StartEliteVR.bat` script to start the emulator and unity project simultaneously

When you first run the project, you will be prompted to allow network access and BeebEm to copy some files to your Documents directory. Accept both pop-ups for correct behaviour.

### Core Functionality Class Diagram
![Unity Class Diagram](/Assets/Images/Software-Manual/UnityRelationships.png)

The Unity project has an `ENet Client` which utilises the data received from the `ENet Server` to update Unity `GameObjects` using `Managers` in the `GameObject` parents which in turn route the data to the relevant individual `Updaters`. The design in general follows the mediator pattern where applicable due to the number of `GameObjects` that require control, providing a single point interface to all 40 ships (20 in game and 20 on minimap display).

### Emulator Display Passthrough
Elite VR obtains a screen capture of the BeebEm window by utilising the `uWindowCapture` plugin. By attaching a `uWindowManager` to any game object in a scene and a `uWindowTexture` to the desired display plane, capture of windows can be texture mapped in game. Elite VR searches for a window containing the text “BBC” in the title to automatically detect the BeebEm window. The repository and full documentation can be found [here](https://github.com/hecomi/uWindowCapture), however the guides are written in Japanese so a translator will need to be used. 

### Scenes
The Game itself is split into 2 scenes, the `Home` scene which acts as the introduction and built in tutorial to the game, followed by the `Immersed` scene which handles the gameplay.

### Scene Transitions
To change scenes, call the `EnterImmersed()` method in the `Trigger_Game_Start` class. By default, this is tied to a callback which occurs after the introduction sequence is complete (touching the floating Elite logo).

When changing scenes, all objects in the current scene will be deleted. If developers wish for an object to persist to the next scene, the `Dont_Destroy_On_Load` script can simply be attached to the desired `GameObject` and it will be carried through to the next scene.

### Other Utility Scripts
For simplicity, some basic scripts have been created for simple and frequently used operations. These scripts simply need to be attached to the GameObject a developer wishes to have the property applied to:

-	`Make_Transparent`: Reduces the alpha channel of the `GameObject`’s material to a desired value 
-	`Respawnable`: Resets a `GameObject`’s position if it exceeds provided limits
-	`Flash_Object`: Makes a GameObject flash at a provided interval
-	`Flash_Text`: Makes text flash at a provided interval
-	`Rotator`: Rotates a `GameObject` at a completely parameterised rate on all 3 Euler angles

### Miscellaneous Notes
#### Applying Main Thread Only Operations
It is not possible to change models or transforms from anywhere outside of the main thread. This means these operations can only be called from inside the update routine or by calling a function from the update routine. Resultantly, the convention in Elite VR is to have a private member variable which can be set by the effector function in conjunction with a state change check within the `Update()` method of the `monobehaviour`. Developers should check if this value has changed and, if so, execute the desired code or call the implemented main thread only function.

##### Example Usage
###### Shield_Controller.cs
> ```
> // Update is called once per frame
> void Update() {
>     if(updated) {
> 	    // Change material here
> 	    ChangeColours(frontShieldStatus, frontLeft, frontRight);	
> 	    ChangeColours(aftShieldStatus, aftLeft, aftRight);
> 	    updated = false;
>     }
> }
> 
> public void UpdateValue(int front, int aft) {
>     frontShieldStatus = front;
>     aftShieldStatus = aft;
>     updated = true;
> }
> 
> private void ChangeColours(int status, Renderer left,   Renderer right) {
>     string targetColour;
>     if(status < 5) {
>         targetColour = "white"; // Shield (essentially) empty
>     } else if(status < 100) {
>         targetColour = "red"; // Shield low
>     } else if(status < 254) {
>         targetColour = "yellow"; // Shield not at maximum
>     } else {
>         targetColour = "green"; // Shield at maximum
>     }
> 
>     // Update the material to the new colour
>     left.material = colourManager.GetColour(targetColour);
>     right.material = colourManager.GetColour(targetColour);
> }
> 
> ```