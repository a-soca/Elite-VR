#pragma once

#include "ENetServer.h"
#include "InputSimulationFacade.h"
#include "EliteFacade.h"
#include "EmulatorController.h"
#include "EliteStreamer.h"
#include <mutex>

//////////////////////////////////////////////////////////////
// C++/C# Cross Compatible ENet Server written by Alex Soca //
//////////////////////////////////////////////////////////////

#define PORT 1337 // Connection Port
#define IP "127.0.0.1" // IP (Localhost)

static ENetHost* server = NULL; // Stores the pointer to the server

bool ENetInitialised = false;
extern bool RunningServer = false; // Used to check if the server is running the main loop

std::mutex broadcastMutex; // Prevent simultaneous transmission/receipt of packets (fix for heap corruption bug)

// Function for sending a packet containg a string to clients
void BroadcastStringFromServer(std::string message, int channel)
{
    const char* data = message.c_str(); // Convert the message to a const char array
    ENetPacket* packet = enet_packet_create(data, strlen(data), ENET_PACKET_FLAG_NONE); // Create the packet
    if (packet == NULL) { return; } // Do not proceed if failed to create packet
    broadcastMutex.lock(); // Lock the mutex until the packet has sent
    enet_host_broadcast(server, channel, packet); // Send the packet
    broadcastMutex.unlock(); // Unlock the mutex to allow other threads to send packets
}

// Processes the logic for received packets
void HandlePacket(char* data, int channel) {
    std::string translated(data); // Convert the char* to a string

    switch (channel) {
        case 0: // Server Commands
            if (translated == "Stream Ship Data") {
                // Start the streamers for ship and player data
                std::thread(EliteStreamer::StreamShipData).detach();
                std::thread(EliteStreamer::StreamPlayerData).detach();
                return;
            }
            else if (translated == "Load Elite" && mainWin != nullptr) {
                // If the main beebem window has been created,
                EmulatorController::LoadDisk("elite-compendium-bbc-micro.dsd"); // Load elite
                return;
            }
            else if (translated == "Shut Down") {
                // Signal the server to shut down (will also close BeebEm)
                RunningServer = false;
                return;
            }
            break;
        case 1: // User input key press
            InputSimulator::PressKey(VkKeyScan(data[0]));
            break;
        case 2: // User input key hold
            InputSimulator::HoldKey(VkKeyScan(data[0]));
            break;
        case 3: // User input key release
            InputSimulator::ReleaseKey(VkKeyScan(data[0]));
            break;
        case 4: // User input arrow key press
            InputSimulator::PressArrowKey(data[0]);
            break;
        case 5: // User input arrow key hold
            InputSimulator::HoldArrowKey(data[0]);
            break;
        case 6: // User input arrow key release
            InputSimulator::ReleaseArrowKey(data[0]);
            break;
        case 7: // User input function key press
            {
                int functionNumber = stoi(translated); // Convert the string to an int
                InputSimulator::PressFunctionKey(functionNumber);
            } 
            break;
        case 8: // User input return key press
            InputSimulator::PressKey(VK_RETURN);
            break;
        case 9: // Analog X input
            {
                int xMag = stoi(translated);
                EliteFacade::SetPlayerRoll(xMag);
            }
            break;
        case 10: // Analog Y input
            {
                int yMag = stoi(translated);
                EliteFacade::SetPlayerPitch(yMag);
            }
        break;
    }
}

// Initialises ENet if not already initialised
int InitialiseENet() {
    if (ENetInitialised) { // Prevent ENet from being initialised twice
        fprintf(stderr, "ENet is already initialised\n"); // Print an error to error io stream
        return 1; // This is an acceptable path but return a different value to let the function caller know
    } 

    if (enet_initialize() != 0) { // If enet initialise fails,
        fprintf(stderr, "enet_initialize failed\n"); // Print an error to error io stream
        exit(EXIT_FAILURE); // Exit with failure code
    } else {
        ENetInitialised = true; // Store the fact that ENet has been initialised
    }

    atexit(enet_deinitialize); // Free up any enet resources at exit
    return 0; // Success
}

// Creates an ENet Host
ENetHost* CreateServer() {
    ENetAddress address; // The address specification for the clients

    enet_address_set_hostname(&address, IP); // Set the ip address of the server

    address.ipv6 = ENET_HOST_ANY; // Tell the server to accept connections from any IP
    address.port = PORT; // Specify the communication port

    ENetHost* host = enet_host_create(
        &address, // Server address
        2, // Number of incoming connections
        11, // Number of channels
        0, // Incoming bandwidth (unlimited)
        0, // Outgoing bandwidth (unlimited)
        4096 // Buffer size
    );

    if (host == NULL) { // If the server was not created
        fprintf(stderr, "enet_host_create failed to create server\n");
        exit(EXIT_FAILURE); // Exit with failure code
    }

    return host;
}

// Initialises ENet and Creates the server
void StartServer() {
    InitialiseENet(); // Initialise ENet (internally protected from double initialisation)
    
    if (server == NULL) { // If the server has not already been created,
        server = CreateServer(); // Create an ENet Host
    }

    RunServer(); // Start the Server
}

// Runs the main server loop, listening to packets on the port and IP specified in StartServer
void RunServer() { // Prevent 2 servers from being run simultaneously
    if (RunningServer) { 
        fprintf(stderr, "Server is already running\n");
        return; 
    } 

    RunningServer = true;

    // Main Server Loop
    while (RunningServer) {
        broadcastMutex.lock();
        ENetEvent event; // Stores the last packet dequeued
        int serviceResult = enet_host_service(server, &event, 0);
        broadcastMutex.unlock();
        if (serviceResult > 0) {
            if (&event == NULL) { break; }
            switch (event.type) {
                case ENET_EVENT_TYPE_CONNECT:
                    break;
                case ENET_EVENT_TYPE_DISCONNECT: // If a client disconnects
                    RunningServer = false; // Exit from the loop
                    break;
                case ENET_EVENT_TYPE_RECEIVE:
                    HandlePacket((char*) event.packet->data, event.channelID); // Interpret the packet
                    enet_packet_destroy(event.packet); // Destroy the packet after completion
                    break;
            }
        }
    }
    // End of server loop

    StopServer(); // Clean up resources

    exit(0); // Shut Down BeebEm
}

// Cleans up resources used by the Server
int StopServer() {
    enet_host_destroy(server);
    return EXIT_SUCCESS;
}