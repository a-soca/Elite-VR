// Alex Soca 24/04/2025
// Class to act as an interface for accessing memory from the emulator parasite
#pragma once

#include "MemoryAccessFacade.h"
#include "BeebWin.h" // For TubeReadMem

// ------------------------ READING ---------------------------------

// Gets a singly byte from the memory of the parasite
int MemoryAccessor::GetByteFromMemory(int address) { // Alex Soca 30/03/2021
	int currentByte = TubeReadMem(address); // Read the memory of the parasite
	if (currentByte > 0xFF || currentByte <= 0) // Clamp between 0-255
		return 0;

	return (int)currentByte; // Return the byte obtained from the parasite memory
}

// Gets a given number of bytes from the memory of the parasite
int* MemoryAccessor::GetMemory(int address, int numBytes) { // Alex Soca 30/03/2021
	// Check number of bytes requested is reasonable
	if (numBytes <= 0 || numBytes > 256)
		exit(1); // Exit if invalid

	// Allocate memory for an array of requested number of bytes size
	int* memory = (int*)calloc(numBytes, sizeof(int));

	// If memory failed to allocate
	if (!memory)
		exit(2); // Exit with error

	for (int i = 0; i < numBytes; i++) // For the number of bytes requested,
		memory[i] = GetByteFromMemory(address + i);

	return memory;
}


// ------------------------ WRITING ---------------------------------

// Writes a byte to the parasite's memory at the provided address
void MemoryAccessor::WriteToMemory(int address, int value) { // Alex Soca 06/04/2025
	TubeWriteMem(address, value); // Parasite memory write function
}