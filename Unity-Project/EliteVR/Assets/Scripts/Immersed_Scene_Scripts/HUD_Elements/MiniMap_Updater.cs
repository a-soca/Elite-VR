using UnityEngine;
using System.Collections.Generic;

public class MiniMap_Updater : MonoBehaviour
{
    // Alex Soca
    // Updates the minimap using the positions of the objects in the immersed scene
    private const int numShips = 20; // Number of ships in scene (including planet and station)
    public GameObject shipContainer; // Parent of the ships
    private List<Transform> mainShips; // List of the ship object transforms
    private List<Transform> miniMapShips; // List of the minimap ship object transforms

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialise the lists
        mainShips = new List<Transform>();
        miniMapShips = new List<Transform>();

        // Find the main and minimap ships, store their transforms for future reference
        for(int i = 1; i < numShips; i++) { // Start at 1 to skip the planet
            mainShips.Add(shipContainer.transform.Find("Slot" + i));
        }

        for(int i = 0; i < numShips-1; i++) { // Find all of the ship minimap objects
            miniMapShips.Add(transform.Find("ScannerSlot" + i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For all ships, update the position and rotation
        for(int i = 0; i < numShips-1; i++) {
            UpdateMinimapShip(i);
        }
    }

    void UpdateMinimapShip(int shipNumber) {
        // Update the ship model to match the main ship
        int ID = mainShips[shipNumber].transform.GetComponent<Ship_Model_Updater>().GetID();
        miniMapShips[shipNumber].transform.GetComponent<Ship_Model_Updater>().SetModelByID(ID);

        // Update position to match main ship and offset by the minimap container position
        miniMapShips[shipNumber].transform.position = mainShips[shipNumber].transform.position/100000 + this.transform.position; 
        miniMapShips[shipNumber].transform.rotation = mainShips[shipNumber].transform.rotation; // Update rotation to match main ship

        // Find each mesh renderer in the minimap ship object
        foreach (MeshRenderer mr in miniMapShips[shipNumber].transform.GetComponentsInChildren<MeshRenderer>()) {
            var shipPositionUpdater = mainShips[shipNumber].GetComponent<Ship_Position_Updater>();
            mr.enabled = shipPositionUpdater.active && shipPositionUpdater.scannerVisibility; // Set object visibility based on if ship is killed or not and if it is visible on scanner
        } 
    }
}
