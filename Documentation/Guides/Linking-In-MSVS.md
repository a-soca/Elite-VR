# Linking Libraries in MSVS
Guide for linking external libraries to a MSVS `.sln` file

## 1: Open sln file
- Must have at least one `.cpp` file in the src folder to show C++ settings
Project structure should look like this:

![Step1](/Assets/Images/Linking-In-MSVS/ProjectStructure.PNG)

## 2: Right click sln and go to Properties

![Step2](/Assets/Images/Linking-In-MSVS/Properties.PNG)

## 3: Navigate to C/C++

![Step3](/Assets/Images/Linking-In-MSVS/CppProperties.PNG)

## 4: Add "Additional Include Directories"
- Click on the arrow to the right
- Click the `<Edit>` option from the dropdown

![Step4](/Assets/Images/Linking-In-MSVS/AdditionalInclude.PNG)

## 5: Edit the Additional Include Properties
- Click the folder icon in the opened window
- Click on the three dots that appear in the selection box

![Step5](/Assets/Images/Linking-In-MSVS/EditInclude.PNG)

## 6: Navigate to the include folder of the library to Link
- Select the folder
- Click OK

![Step6](/Assets/Images/Linking-In-MSVS/LibraryLocation.PNG)

## 7: Click Apply then OK

![Step7](/Assets/Images/Linking-In-MSVS/Apply.PNG)

## Break
At this point, the headers have been imported, however the library is still yet to be included. It may seem like you can use functions from the library, but there will be linking errors at compile time:

![Break](/Assets/Images/Linking-In-MSVS/LinkErrors.PNG)

### Static linking
- Static linking builds the library into the executable
- Uses `.lib`
- Ensure the library you are importing matches the architecture of the project (i.e. use the x64 version if compiling x64 applications)

## 8: Open Properties
- Right click the solution and go to properties

![Step8](/Assets/Images/Linking-In-MSVS/Properties.PNG)

## 9: Navigate to Linker Settings

![Step9](/Assets/Images/Linking-In-MSVS/LinkerProperties.PNG)

## 10: Go to General
- Click on "Additional Library Directories"

![Step10](/Assets/Images/Linking-In-MSVS/LinkerGeneralProperties.PNG)

## 11: Add Additional Library Directories
- Click the down arrow
- Click the `<Edit>` option in the dropdown

![Step11](/Assets/Images/Linking-In-MSVS/AdditionalLibrary.PNG)

## 12: Find the `lib` folder to Link
- Similar to the Include, click the folder icon
- Click on the three dots that appear
- Navigate to the `lib` folder
- Click OK

![Step12](/Assets/Images/Linking-In-MSVS/StaticLibraryLocation.PNG)

## 13: Go to the Input section

![Step13](/Assets/Images/Linking-In-MSVS/LinkerInputProperties.PNG)

## 14: Add the Additional Dependency
- Go to the start of the dependency string
- Specify the correct library name
- Add a semicolon afterwards to separate from the other libraries

![Step14](/Assets/Images/Linking-In-MSVS/AddAdditionalDependency.PNG)

## 15: Apply changes
- Click apply
- Click OK