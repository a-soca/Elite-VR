#pragma once

#include <enet/enet.h>
#include <string>

#include "BeebWin.h"
#include "Main.h"

#include <iostream>

#pragma comment(lib, "WinMM.Lib")
#pragma comment(lib, "enet.dll")

#include <thread>
#include "Debug.h"

/*
C++/C# Cross Compatible ENet Server written by Alex Soca
*/

// Server Management Functions
void StartServer(); // Initialises ENet, Creates and Runs the server
int InitialiseENet(); // Initialises ENet if not already initialised
ENetHost* CreateServer(); // Creates an ENet Host
void RunServer(); // Runs the main server loop, listening to packets on the port and IP specified in StartServer
int StopServer(); // Cleans up resources used by the Server

// Server Commmunication Functions
void HandlePacket(char* data, int channel); // Processes the logic for received packets
void BroadcastStringFromServer(std::string message, int channel); // Function for sending strings to clients on specific channels

extern bool RunningServer;