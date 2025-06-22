#pragma once

#include "BeebWin.h"

// Alex Soca 24/04/2025
// Class used to configure and control the emulator

class EmulatorController {
public:
	static void SetEmulatorSpeed(int speed);
	static void LoadDisk(const char* fileName);
};