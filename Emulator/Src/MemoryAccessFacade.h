#pragma once

// Alex Soca 24/04/2025
// Class to act as an interface for accessing memory from the emulator parasite

class MemoryAccessor {
public:
	static int GetByteFromMemory(int address); // Gets a singly byte from the memory of the parasite
	static int* GetMemory(int address, int numBytes); // Gets a given number of bytes from the memory of the parasite
	static void WriteToMemory(int address, int value); // Writes a byte to the parasite's memory at the provided address
};