// Alex Soca 24/04/2025
// Class to stream data from Elite to the Client using the Server

#include "EliteStreamer.h"

// Streams coordinates and orientation vectors to the client
void EliteStreamer::StreamShipData() { // Alex Soca, 25/02/2025
	EliteFacade::AddDockingComputer(); // Add the docking computer by default
	EliteFacade::AddECM(); // Add ECM by default

	int numSlots = EliteFacade::GetNumShips();
	int address = EliteFacade::GetStartOfShipDataBlocks();
	const int shipSlotLength = EliteFacade::GetNumBytesPerShipBlock(); // Number of bytes in each ship slot

	char buffer[4096]; // Create a buffer to store the formatted data

	int numShipsOnScreen = 0; // Initialise variable to store the number of ships being rendered on emulator screen

	int scannerVisibility = 0; // Initialise the scanner visibility bit
	int exploding = 0; // Initialise the exploding bit
	int firingLasers = 0; // Initialise the firing laser bit
	int killed = 1; // Initialise the killed bit
	int* coordinates = new int[3]; // Array to store xyz coordinates of ship
	int* vectors = new int[9]; // Array to store orientation vectors of ship
	std::string dataBlock; // Create a string to store the extracted data

	while (RunningServer) { // previously !exitFlag.load()
		int prevShipsOnScreen = numShipsOnScreen; // Store the previous number of ships on screen
		numShipsOnScreen = 0; // Reset the number of ships on screen counter

		dataBlock = ""; // Reset the coordinate block string
		for (int i = 0; i < numSlots; i++) {
			int shipID = EliteFacade::GetShipID(i);

			if (shipID == 0) {
				break; // If the ship ID is 0, the slot (and every slot after it) is empty so we can just end the search.
			}

			// Get the memory values in the ship slot
			int* memory = MemoryAccessor::GetMemory(address // Starting address 
				+ i * shipSlotLength, // Offset by the slot number
				shipSlotLength); // The number of bytes to retreive

			EliteFacade::ShipIsKilled(memory, killed); // Check if the ship is killed
			if (killed) { // If the ship is killed, just set all values to 0 as the object will be turned off
				memset(coordinates, 0, sizeof(int) * 3);
				memset(vectors, 0, sizeof(int) * 9);
			}
			else {
				EliteFacade::GetShipCoordinates(memory, coordinates); // Get and translate the coordinate bytes
				EliteFacade::GetShipVectors(memory, vectors); // Get and translate the orientation vector bytes
				EliteFacade::ShipIsVisibleOnScanner(memory, scannerVisibility);
				EliteFacade::ShipIsExploding(memory, exploding); // Check if the ship is exploding
				EliteFacade::ShipIsFiringLasers(memory, firingLasers); // Check if the ship is firing lasers
			}

			if (EliteFacade::ShipIsBeingDrawnOnScreen(memory)) {
				numShipsOnScreen++;
			}

			/*
			Convert the coordinates into a formatted string
			Format: [A]|[B]|[C]|[D]|[E]|[X],[Y],[Z]|[NX],[NY],[NZ]|[RX],[RY],[RZ]|[SX],[SY],[SZ]
			[A] = Ship ID
			[B] = Visible on scanner (1/0)
			[C] = Exploding (1/0)
			[D] = Firing Lasers (1/0)
			[E] = Ship Killed (1/0)
			[X] = X coordinate
			[Y] = Y coordinate
			[Z] = Z coordinate
			[NX] = Nose X Vector
			[NY] = Nose Y Vector
			[NZ] = Nose Z Vector
			[RX] = Roof X Vector
			[RY] = Roof Y Vector
			[RZ] = Roof Z Vector
			[SX] = Side X Vector
			[SY] = Side Y Vector
			[SZ] = Side Z Vector
			Example: 2|1|0|0|0|-100,20,400|-20,2,6|14,20,-50|90,45,-10` = Ship with ID 2 is visible on scanner, not exploding, firing lasers or killed and at position (-100, 20, 400) with nose vector (-20, 2, 6), roof vector (14, 20, -50) and side vector (90, 45, -10)
			Each ship block is delimited with a \n char, therefore by splitting the string by newline we can retrieve the ship slot by the index in the split array
			*/

			// Format the coordinates and vectors as specified
			int length = snprintf(buffer, sizeof(buffer), "%d|%d|%d|%d|%d|%d,%d,%d|%d,%d,%d|%d,%d,%d|%d,%d,%d\n",
				shipID, // The ID of the ship (which model to use)
				scannerVisibility, // The bit which indicates if a ship is visible on the scanner
				exploding, // The bit which indicates if a ship is exploding
				firingLasers, // The bit which indicates if a ship is firing lasers
				killed, // The bit which indicates if a ship has been killed

				coordinates[0], // The x coordinate of the ship slot number
				coordinates[1], // The y coordinate of the ship slot number
				coordinates[2], // The z coordinate of the ship slot number

				vectors[0], // Nose vector X
				vectors[1], // Nose vector Y
				vectors[2], // Nose vector Z

				vectors[3], // Roof vector X
				vectors[4], // Roof vector Y
				vectors[5], // Roof vector Z

				vectors[6], // Side vector X
				vectors[7], // Side vector Y
				vectors[8]  // Side vector Z
			);

			std::string formattedData(buffer); // Create a string from the buffer
			dataBlock += formattedData; // Append the string with the new ship block data
		}

		// If the number of ships on screen has changed,
		if (numShipsOnScreen != prevShipsOnScreen) {
			int speed = 100 + 20 * numShipsOnScreen;
			EmulatorController::SetEmulatorSpeed(speed); // Set the emulator speed to the value corresponding to the number of ships on screen
		}

		if (dataBlock != "") { // If the data block is not empty, stream it
			BroadcastStringFromServer(dataBlock.c_str(), 2); // Broadcast the packet to clients on channel 2 (ship data channel)
			std::this_thread::sleep_for(std::chrono::milliseconds(25)); // Do not reduce below 25
		}
	}
}

void EliteStreamer::StreamPlayerData() {
	char buffer[4096]; // Create a buffer to store the formatted data
	while (RunningServer) {

		// Get the movement information of the player's ship from the zero page
			// Format: [S]|[P]|[R]|[FS]|[AS]|[E]|[CT]|[LT]|[AL]|[FV]|[DC]|[DK]|[HS]|[NM]|[LS]|[LT]|[EA]|[MS]
			// [S] = Speed
			// [P] = Pitch
			// [R] = Roll
			// [FS] = Front Shield
			// [AS] = Aft Shield
			// [E] = Energy
			// [CT] = Cabin Temperature
			// [LT] = Laser Temperature
			// [AL] = Altitude
			// [FV] = Fuel Volume
			// [DC] = Docking Computer Status
			// [DK] = Docked Status
			// [HS] = Hyperspace Timer
			// [NM] = Number of Missiles Fitted
			// [LS] = Lock Searching for Target
			// [LT] = Lock Target
			// [EA] = ECM Active
			// [MS] = Menu Screen
			

		snprintf(buffer, sizeof(buffer), "%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d|%d",
			// Telemetry
			EliteFacade::GetPlayerSpeed(), // Speed
			EliteFacade::GetPlayerPitch(), // Pitch
			EliteFacade::GetPlayerRoll(), // Roll

			// Combat Info
			EliteFacade::GetPlayerFrontShield(), // Front Shield status
			EliteFacade::GetPlayerAftShield(), // Aft Shield Status
			EliteFacade::GetPlayerEnergy(), // Energy (health)

			// Temps
			EliteFacade::GetPlayerCabinTemp(), // Cabin Temperature
			EliteFacade::GetPlayerGunTemp(), // Laser Temperature

			// Other Critical Information
			EliteFacade::GetPlayerAltitude(), // Altitude
			EliteFacade::GetPlayerFuelVolume(), // Fuel Volume
			EliteFacade::GetDockingComputerStatus(), // Docking Computer active/inactive
			EliteFacade::IsShipDocked(), // Ship Docked/Undocked
			EliteFacade::GetHyperspaceTimer(), // Hyperspace countdown value

			// Weapons
			EliteFacade::GetNumberOfMissilesFitted(),// [NM] = Number of Missiles Fitted
			EliteFacade::GetTargetingActive(), // [LS] = Lock Searching for Target
			EliteFacade::GetMissileLockTarget(), // [LT] = Lock Target
			EliteFacade::GetECMStatus(), // [EA] = ECM Active

			EliteFacade::GetMenuScreen() // [MS] = The Menu Screen Being Viewed
		);

		std::string formattedData(buffer); // Create a string from the values
		BroadcastStringFromServer(formattedData.c_str(), 3); // Send the player ship data on the relevant channel
		std::this_thread::sleep_for(std::chrono::milliseconds(75)); // Do not reduce below 25
	}
}


