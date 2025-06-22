#pragma once

// Alex Soca 24/04/2025
// Class to stream data from Elite to the Client using the Server

#include "ENetServer.h"
#include "EliteFacade.h"
#include "EmulatorController.h"

class EliteStreamer {
public:
	static void StreamShipData();
	static void StreamPlayerData();
};