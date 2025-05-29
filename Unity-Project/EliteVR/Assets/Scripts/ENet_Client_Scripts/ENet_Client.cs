using UnityEngine;
using ENet;
using System.Text;
using System.Linq;
using System.Threading;
using System.Collections;

public class ENet_Client : MonoBehaviour
{
    // Alex Soca
    // C#/C++ Cross Compatible ENet Client
    
    const string IP = "127.0.0.1"; // The IP to connect to (localhost)
    const int PORT = 1337; // The communication port
    const int BUFFER_SIZE = 4096; // Size of each packet buffer
    Host client; // The Unity node (client side)
    Address address; // The address of the server
    Peer peer; // The Emulator node (server side)
    ENet.Event netEvent; // Object to hold data recieved from server

    public Elite_Position_Manager positionManager; // Used to move gameobjects
    public Explosion_Manager explosionManager; // Used to make explosions reactive to player movement
    public Space_Dust_Updater spaceDustUpdater; // Used to make space dust reactive to player movement
    public HUD_Updater HUDUpdater; // Used to display information and alerts to the player
    public Critical_Info_Manager criticalInfoManager; // Used to display critical telemetry and health information

    public void SendPacket(string message, int channel) { // Function to send data to the server
        Packet packet = default(Packet); // Create a new packet
        byte[] data = new byte[BUFFER_SIZE]; // Allocate memory for data to add to packet

        message += "\0"; // Append the message with a zero byte for correct decoding in C++ server

        data = Encoding.GetEncoding("UTF-8").GetBytes(message); // Convert the string to a byte array

        packet.Create(data); // Add the data to the packet

        // Send the packet
        if(!peer.Send((byte) channel, ref packet)) {
            Debug.Log("Packet send failed"); // If the packet does not send, print a log message
        }
    }

    string ReadPacket(ENet.Event networkEvent) {
        byte[] buffer = new byte[BUFFER_SIZE]; // Create byte array of suitable size
        networkEvent.Packet.CopyTo(buffer); // Copy packet data to buffer

        return System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0'); // Return the data
    }

    void HandlePacket(ENet.Event networkEvent) {
        string contents = ReadPacket(networkEvent); // Read the packet data and store in a string

        switch(networkEvent.ChannelID) {
            case 2: // Ship Data Channel
                positionManager.UpdatePosition(contents); // Send to the position manager
                break;
            case 3: // Player Ship information
                HUDUpdater.UpdateHUDValues(contents); // Send to the HUD Updater
                spaceDustUpdater.UpdateSpaceDust(contents); // Send to the Particle Updater
                explosionManager.UpdateMovementInformation(contents); // Send to the Particle Updater
                criticalInfoManager.UpdatePanel(contents);
                break;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ENet.Library.Initialize(); // Initialise the enet library

        client = new Host(); // Create a new host to connect to the server (peer)

        // The address of the server
        address = new Address(); // Create address object
        address.SetIP(IP); // Set the IP to localhost
        address.Port = PORT; // Set the port to the specified const

        client.Create(); // Create the client

        peer = client.Connect(address, 11); // Connect the client to the server
    }

    void CheckServer() {
        bool polled = false;

        while(!polled) {
            if (client.CheckEvents(out netEvent) <= 0) { // Check for any net events
				if (client.Service(0, out netEvent) <= 0) { break; }
                polled = true;
            }

            switch (netEvent.Type) { // Depending on the type,
                case ENet.EventType.None:
                    Debug.Log("No Event Type");
                    break;

                case ENet.EventType.Connect:
                    Debug.Log("Client Connected to server : " + peer.IP + ":" + peer.Port + " [" + peer.State + "]");
                    break;

                case ENet.EventType.Disconnect:
                    Debug.Log("Client disconnected from server");
                    break;

                case ENet.EventType.Timeout:
                    Debug.Log("Client connection timeout");
                    break;

                case ENet.EventType.Receive:
                    // string data = ReadPacket(netEvent); // Extract the data from the packet
                    // UnityEngine.Debug.Log("Packet of Length " + netEvent.Packet.Length + " Containing [" + data + "] was Received from Server [IP]:[PORT] " + peer.IP + ":" + peer.Port + " on Channel " + netEvent.ChannelID);
                    HandlePacket(netEvent);
                    netEvent.Packet.Dispose(); // Discard the packet after reading
                    break;
            }
        }
    }

    void OnApplicationQuit() {
        SendPacket("Shut Down", 0); // Request that the server shuts down
        client.Flush(); // Forcefully send the packet
        peer.Disconnect(0); // Disconnect from the server
        ENet.Library.Deinitialize(); // Clean up ENet gracefully
    }

    // Update is called once per frame
    void Update()
    {
        CheckServer(); // Check if new packets have arrived and handle them
    }
}