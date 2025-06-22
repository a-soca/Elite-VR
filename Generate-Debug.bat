@echo off

echo Checking Files are Present...

if not exist "Elite-VR-Release-Dependencies" (
    echo ERROR: Could not locate the Elite VR Release Dependencies folder. Please run this script from the repository root.
    exit
)

if not exist "Emulator/Src/x64/Debug" (
    echo ERROR: BeebEm has not been built in Debug mode. Please compile with the correct target deployment and try again.
    exit
)

if not exist "Unity-Project/Build/EliteVR.exe" (
    echo ERROR: The EliteVR Unity Project has not been built in the correct location. Please build to ^<HERE^>/Unity-Project/Build/ and try again.
    exit
)

echo Success!

echo Checking if Release Already Present...
if exist "Elite-VR-DEBUG" (
    echo Deleting Old Release...
    del Elite-VR-vX.X.X
)
echo Success!


mkdir "Elite-VR-DEBUG"

echo Adding BeebEm to Release...
ROBOCOPY "Emulator/Src/x64/Debug" "Elite-VR-DEBUG/BeebEm" /mir
echo Success!

echo Adding Elite VR Unity Project to Release...
ROBOCOPY "Unity-Project/Build" "Elite-VR-DEBUG/Unity-Build" /mir
echo Success!

echo Removing do-not-ship Folders from Release Unity Build...
del Elite-VR-vX.X.X/Unity-Build/BackUpThisFolder_ButDontShipItWithYourGame
del Elite-VR-vX.X.X/Unity-Build/BurstDebugInformation_DoNotShip
echo Success!

echo Adding Dependencies...
ROBOCOPY "Elite-VR-Release-Dependencies/UserData" "Elite-VR-DEBUG/BeebEm/UserData" /mir
ROBOCOPY "Elite-VR-Release-Dependencies" "Elite-VR-DEBUG" elite-compendium-bbc-micro.dsd
ROBOCOPY "Elite-VR-Release-Dependencies" "Elite-VR-DEBUG/BeebEm" enet.dll
ROBOCOPY "Elite-VR-Release-Dependencies" "Elite-VR-DEBUG" StartEliteVR.bat
echo Done!