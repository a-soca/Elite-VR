#pragma once

// Alex Soca 24/04/2025
// Class to act as an interface for accessing data from Elite 6502 Co Processor Version

#include "EliteFacade.h"

// -------------------- NON PLAYER SHIP DATA ------------------------

// Gets the Ship ID From Memory in .FRIN based on the index provided
int EliteFacade::GetShipID(int shipNum) { // Alex Soca 03/04/2025
	// FRIN + the index = the address of the ship ID Byte
	return MemoryAccessor::GetByteFromMemory(FRIN + shipNum);
}

// Finds the bytes related to coordinates in the ship block provided and translates to coordinates
void EliteFacade::GetShipCoordinates(int* shipBlock, int*& output) { // Alex Soca 30/03/2025
	for (int xyz = 0; xyz < 3; xyz++) {
		int offset = xyz * 3;
		int coordinate = TranslateCoordinates(shipBlock[0 + offset], // lo
											shipBlock[1 + offset], // hi
											shipBlock[2 + offset]); //sign
		output[xyz] = coordinate;
	}
}

// Finds the bytesrelated to orientation vectors in the ship block provided and translates to vector values
void EliteFacade::GetShipVectors(int* shipBlock, int*& output) { // Alex Soca 30/03/2025
	/*
	Vectors will be returned in the format:

	[0] = Nose X
	[1] = Nose Y
	[2] = Nose Z

	[3] = Roof X
	[4] = Roof Y
	[5] = Roof Z

	[6] = Side X
	[7] = Side Y
	[8] = Side Z
	*/

	// For all directions (Nose, Roof, Side),
	for (int direction = 0; direction < 3; direction++) {
		int directionOffset = 6 * direction;

		// For all components (x, y, z)
		for (int xyzOffset = 0; xyzOffset < 3; xyzOffset++) {
			int byteLocation = directionOffset + xyzOffset * 2 + 9;

			// Get the vector
			int vector = TranslateVectors(shipBlock[byteLocation], // lo
										shipBlock[byteLocation + 1]); // hi/sign

			output[direction * 3 + xyzOffset] = vector;
		}
	}
}

// Checks if the provided ship is visible on the screen
bool EliteFacade::ShipIsBeingDrawnOnScreen(int* shipBlock) { // Alex Soca 23/04/2025
	// Bit 3 is the "draw on screen" bit (1 if visible, 0 if not)
	return (shipBlock[31] & 0b00001000) == 0b00001000;
}

// Checks if the provided ship is visible on the scanner
void EliteFacade::ShipIsVisibleOnScanner(int* shipBlock, int& output) { // Alex Soca 02/04/2025
	// Bit 4 is the "show on scanner" bit (1 if visible, 0 if not)
	output = shipBlock[31] & 0b00010000;
}

// Checks if the provided ship is exploding
void EliteFacade::ShipIsExploding(int* shipBlock, int& output) { // Alex Soca 02/04/2025
	// Bit 5 is the "ship exploding" bit (1 if exploding, 0 if not)
	output = shipBlock[31] & 0b00100000;
}

// Checks if the provided ship is firing lasers
void EliteFacade::ShipIsFiringLasers(int* shipBlock, int& output) { // Alex Soca 02/04/2025
	// Bit 6 is the "ship firing lasers" bit (1 if firing, 0 if not)
	// Bit 6 is ALSO the "explosion drawn" bit, but we can ignore this
	// if we prevent lasers from firing while the explosion is being drawn

	output = shipBlock[31] & 0b01000000; // Get the firing bit
}

// Gets the Ship Killed Bit from the Ship Data Block
void EliteFacade::ShipIsKilled(int* shipBlock, int& output) { // Alex Soca 31/03/2025
	output = shipBlock[31] & 0b10000000; // Bit 7 is the "ship killed" bit (1 if killed, 0 if alive)
}

// ----------------------- PLAYER SHIP DATA ------------------------

// Getters

// Gets the Speed of the Player's Ship from the Zero Page in Parasite Memory
int EliteFacade::GetPlayerSpeed() { // Alex Soca 01/04/2025
	return MemoryAccessor::GetByteFromMemory(DELTA); // Get the magnitude of speed from memory (.DELTA)
}

// Gets the Pitch of the Player's Ship from the Zero Page in Parasite Memory
int EliteFacade::GetPlayerPitch() { // Alex Soca 01/04/2025
	int beta = MemoryAccessor::GetByteFromMemory(BETA); // Get the signed pitch value (.BETA)
	int magnitude = beta & 0x7F; // Extract the magnitude
	int sign = beta & 0x80; // Extract the sign value
	return sign == 0 ? magnitude : -magnitude; // If the sign byte is not 0, return negative
}

// Gets the Roll of the Player's Ship from the Zero Page in Parasite Memory
int EliteFacade::GetPlayerRoll() { // Alex Soca 01/04/2025
	int alpha = MemoryAccessor::GetByteFromMemory(ALP1); // Get the magnitude of the roll (.ALPHA)
	int sign = MemoryAccessor::GetByteFromMemory(ALP2) & 0x80; // Get the sign of the roll
	return sign == 0 ? alpha : -alpha; // If the sign byte is not 0, return negative
}

int EliteFacade::GetPlayerFrontShield() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(FSH); // Front Shield status
}

int EliteFacade::GetPlayerAftShield() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(ASH); // Aft Shield status
}

int EliteFacade::GetPlayerEnergy() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(ENERGY); // Energy status
}

int EliteFacade::GetPlayerCabinTemp() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(CABTMP); // Cabin Temperature
}

int EliteFacade::GetPlayerGunTemp() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(GNTMP); // Gun Temperature
}

int EliteFacade::GetPlayerAltitude() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(ALTIT); // Altitude
}

int EliteFacade::GetPlayerFuelVolume() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(QQ14); // Fuel Volume
}

int EliteFacade::GetDockingComputerStatus() { // Alex Soca 24/04/2025
	return MemoryAccessor::GetByteFromMemory(AUTO); // Docking Computer Status
}

int EliteFacade::GetMenuScreen() { // Alex Soca 02/05/2025
	return MemoryAccessor::GetByteFromMemory(QQ11); // The Menu Screen the Player is Currently Looking At
}

int EliteFacade::IsShipDocked() { // Alex Soca 01/05/2025
	return MemoryAccessor::GetByteFromMemory(QQ12); // Docked Status
}

int EliteFacade::GetHyperspaceTimer() { // Alex Soca 24/04/2025
	// QQ22+1 is the second hyperspace timer which indicates the seconds until jump
	return MemoryAccessor::GetByteFromMemory(QQ22+1); // Hyperspace timer
}

int EliteFacade::GetNumberOfMissilesFitted() { // Alex Soca 01/05/2025
	return MemoryAccessor::GetByteFromMemory(NOMSL); // Gets the number of missiles fitted to the ship
}

int EliteFacade::GetTargetingActive() { // Alex Soca 01/05/2025
	return MemoryAccessor::GetByteFromMemory(MSAR); // Gets the active status of the missile targeting system
}

int EliteFacade::GetMissileLockTarget() { // Alex Soca 01/05/2025
	return MemoryAccessor::GetByteFromMemory(MSTG); // Gets the ship slot number of the targeted ship (FF if no target)
}

int EliteFacade::GetECMStatus() {
	return MemoryAccessor::GetByteFromMemory(ECMP);
}

// Setters
void EliteFacade::SetPlayerPitch(int value) { // Alex Soca
	// .JSTY (0x899) stores the current pitch value
	MemoryAccessor::WriteToMemory(JSTY, value);
}

void EliteFacade::SetPlayerRoll(int value) { // Alex Soca
	// .JSTX (0x898) stores the current roll value
	MemoryAccessor::WriteToMemory(JSTX, value);
}

// Adds a docking computer to the loadout
void EliteFacade::AddDockingComputer() { // Alex Soca, 06/04/2025
	// .DKCMP 0x8D0, FF is fitted, 0 is unfitted
	MemoryAccessor::WriteToMemory(DKCMP, 0xFF);
}

// Adds an ECM to the loadout
void EliteFacade::AddECM() { // Alex Soca, 03/05/2025
	MemoryAccessor::WriteToMemory(ECM, 0xFF);
}

// --------------------------- GAME DATA ----------------------------

int EliteFacade::GetStartOfShipDataBlocks() {
	return KP;
}

int EliteFacade::GetNumShips() {
	return NUM_SHIPS;
}

int EliteFacade::GetNumBytesPerShipBlock() {
	return SHIP_BLOCK_LENGTH;
}


// ---------------------- TRANSLATION UTILITIES ---------------------

// Translates raw bytes to single coordinate
int EliteFacade::TranslateCoordinates(int lo, int hi, int sign) { // Alex Soca 30/03/2025
	hi = hi << 8; // Bitshift hi left by 2 hex places (1 Byte)
	int hi2 = (sign & 0x7F) << 16; // Extract second hi byte (up to 7-bit) and bitshift left by 4 hex places (2 Bytes)
	sign = sign & 0x80; // Extract bit 7 from the sign byte to obtain +/-
	int magnitude = hi2 + hi + lo; // Sum the hi and lo bytes to retrieve the full value

	return sign == 0 ? magnitude : -magnitude; // Return the coordinate and change the magnitude if necessary
}

// Translates raw bytes to single vector
int EliteFacade::TranslateVectors(int lo, int hiSign) { // Alex Soca 30/03/2025
	int hi = (hiSign & 0x7F) << 8; // Extract hi byte (up to 7-bit) and bitshift left by 2 hex places
	int sign = hiSign & 0x80; // Extract sign bit

	int magnitude = hi + lo;  // Combine into full value

	return sign == 0 ? magnitude : -magnitude;  // Apply sign if needed
}