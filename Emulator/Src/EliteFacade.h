#pragma once

#include "MemoryAccessFacade.h"

class EliteFacade {
public:
	// Memory Access for Ships
	static int GetShipID(int shipNum); // Alex Soca 03/04/2025
	static void GetShipCoordinates(int* shipBlock, int*& output); // Alex Soca 30/03/2025
	static void GetShipVectors(int* shipBlock, int*& output); // Alex Soca 30/03/2025
	static bool ShipIsBeingDrawnOnScreen(int* shipBlock); // Alex Soca 23/04/2025
	static void ShipIsVisibleOnScanner(int* shipBlock, int& output); // Alex Soca 02/04/2025
	static void ShipIsExploding(int* shipBlock, int& output); // Alex Soca 02/04/2025
	static void ShipIsFiringLasers(int* shipBlock, int& output); // Alex Soca 02/04/2025
	static void ShipIsKilled(int* shipBlock, int& output); // Alex Soca 31/03/2025

	// Player Ship Telemetry
	static int GetPlayerSpeed(); // Alex Soca 01/04/2025
	static int GetPlayerPitch(); // Alex Soca 01/04/2025
	static int GetPlayerRoll(); // Alex Soca 01/04/2025

	// Player Ship Info
	static int GetPlayerFrontShield(); // Alex Soca 24/04/2025
	static int GetPlayerAftShield(); // Alex Soca 24/04/2025
	static int GetPlayerEnergy(); // Alex Soca 24/04/2025

	// Other Critical Info
	static int GetPlayerCabinTemp(); // Alex Soca 24/04/2025
	static int GetPlayerGunTemp(); // Alex Soca 24/04/2025
	static int GetPlayerAltitude(); // Alex Soca 24/04/2025
	static int GetPlayerFuelVolume(); // Alex Soca 24/04/2025

	// Player Ship Status
	static int GetDockingComputerStatus(); // Alex Soca 24/04/2025
	static int IsShipDocked(); // Alex Soca 01/05/2025
	static int GetHyperspaceTimer(); // Alex Soca 24/04/2025

	// Weapons
	static int GetNumberOfMissilesFitted(); // Alex Soca 01/05/2025
	static int GetTargetingActive(); // Alex Soca 01/05/2025
	static int GetMissileLockTarget(); // Alex Soca 01/05/2025
	static int GetECMStatus(); // Alex Soca 03/05/2025

	static void SetPlayerPitch(int value); // Alex Soca 06/04/2025
	static void SetPlayerRoll(int value); // Alex Soca 06/04/2025

	// Cheats for Equipment
	static void AddDockingComputer(); // Alex Soca, 06/04/2025
	static void AddECM(); // Alex Soca, 03/05/2025

	static int GetMenuScreen(); // Alex Soca, 02/05/2025

	// Game Data
	static int GetStartOfShipDataBlocks();
	static int GetNumShips();
	static int GetNumBytesPerShipBlock();

private:
	// Alex Soca, 08/04/2025 - 13/04/2025
	// Constant memory addresses for the Elite 6502 Co Processor Version
	// Utility
	static const int FRIN = 0x852; // The ship data block pointer address
	static const int KP = 0x8200; // The start of the k% memory address (start of ship data blocks)
	static const int NUM_SHIPS = 20; // For the second processor version
	static const int SHIP_BLOCK_LENGTH = 37; // Number of bytes per ship block

	// Player Ship Information
	static const int ALP1 = 0x30; // Magnitude of roll
	static const int ALP2 = 0x31; // Sign of roll
	static const int BETA = 0x29; // Signed pitch value
	static const int DELTA = 0x7C; // Magnitude of speed
	static const int QQ11 = 0x86; // Stores the current view of the player in menus
	static const int QQ12 = 0x90; // Stores the docked status of the players ship (0 if not docked, FF if docked)

	// Weapons
	static const int MSTG = 0x44; // Missile Locked Target Ship (FF if none, 0-20 depending on the slot of the target) 
	static const int NOMSL = 0x8D7; // Number of missiles currently fitted from 0-4
	static const int MSAR = 0x890; // Missile Targeting Active (FF if true, 0 if false)
	static const int ECMP = 0x88C; // ECM Active (0 if false, otherwise true)

	// Combat Info
	static const int FSH = 0x8F1; // Front Shield, 0-255 where 0 is empty and 255 is full
	static const int ASH = 0x8F2; // Aft Shield, 0-255 where 0 is empty and 255 is full
	static const int ENERGY = 0x8F3; // Energy (health), 0-255 where 0 is empty and 255 is full
	static const int CABTMP = 0x88E; // Cabin Temperature, Ambient is 30, Increases as ship approaches a sun up to 255
	static const int GNTMP = 0x893; // Laser Temperature, If this excedes 242, the laser overheats and cannot be fired until it cools down
	
	// Other Critical Info
	static const int ALTIT = 0xE35; // Altitude from sun/planet, Range from 1-255, clips to 255 if a large distance from planet or sun, 0 if crashed into surface
	static const int QQ14 = 0x8B1; // Fuel Volume, Range from 0-70 measured in light years*10 (1 = 0.1 light years, 70 = 7 light years)
	static const int AUTO = 0x88B; // Docking computer active flag (0 if off, otherwise on)
	static const int QQ22 = 0x2D; // Hyperspace Countdown Timer

	// Player Ship Equipment
	static const int DKCMP = 0x8D0; // Docking Computer, FF if fitted, 0 if unfitted
	static const int ECM = 0x8CC; // ECM installed address (0 for not installed, 0xFF for installed) Jianhao Chen/Haolin Zhang

	// Player Input
	static const int JSTX = 0x898; // Joystick X position
	static const int JSTY = 0x899; // Joystick Y position

	// Memory Translation for Ships
	static int TranslateCoordinates(int lo, int hi, int sign); // Alex Soca 30/03/2025
	static int TranslateVectors(int lo, int hiSign);
};