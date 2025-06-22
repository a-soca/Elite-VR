using UnityEngine;
using System.Text;
using System.Linq;
using System.Collections.Generic;

public class Elite_Position_Manager : MonoBehaviour
{
    // Alex Soca
    // Master position manager for all ships
    private Ship_Position_Updater[] positionUpdaters; // An array of all the position update scripts attached to ship objects
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the ENet Client script and set this script as the position manager
        GameObject.Find("ENetClient").GetComponent<ENet_Client>().positionManager = this;

        positionUpdaters = this.GetComponentsInChildren<Ship_Position_Updater>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePosition(string packetContents) {
        if(packetContents.Contains('\n')) {
            var shipBlocks = packetContents.Split('\n'); // Split up each line to a separate ship data block
        
            // For each ship in the packet,
            for(int i = 0; i < shipBlocks.Length-1; i++) {
                positionUpdaters[i].UpdatePosition(shipBlocks[i]); // Update the position
                positionUpdaters[i].Activate();
            }

            for(int i = shipBlocks.Length-1; i < 20; i++){
                positionUpdaters[i].Deactivate();
            }
        } else if(packetContents.Contains('|')) {
            positionUpdaters[0].UpdatePosition(packetContents); // Update the position of the planet
            for(int i = 1; i < 20; i++){
                positionUpdaters[i].Deactivate();
            }
        }
    }
}
