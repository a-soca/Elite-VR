using UnityEngine;
using static Ship_Colours;
using static Scanner_Colours;

public class Ship_Colour_Updater : MonoBehaviour
{
    // Alex Soca
    // Script for updating colours of ships based on their ID
    public bool scanner = false; // Tick box for if the updater is attached to a scanner ship model

    private Renderer wireframeRenderer; // Used to change the colour of the wireframe

    // Variables to store the ID of the ship
    private int currentID = 0;
    private int ID = 0;

    private Ship_Model_Updater modelUpdater; // Used to get the ID if the ship
    private Colour_Manager colourManager; // Used to get the material of the requested colour

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        modelUpdater = this.transform.GetComponent<Ship_Model_Updater>(); // Get the model updater attached to the script
        wireframeRenderer = this.transform.GetChild(1).GetComponent<Renderer>(); // Get the renderer for the wireframe
        colourManager = this.transform.GetComponentInParent<Colour_Manager>(); // Get the colour manager from the ship container
    }

    // Update is called once per frame
    void Update()
    {
        ID = modelUpdater.GetID(); // Update the ID

        if(currentID != ID) { // If the ID has changed,
            currentID = ID; // Update the current ID

            // Translate the ship ID to an index
            int index;
            if(ID > 34 || ID < 1) { // If the ID is not in the first 34, set it to 34 (Planets/Sun)
                index = 34;
            } else {
                index = ID-1; // Translate the ID into an index
            }

            // Update the colour
            if(scanner) { // If the object is a scanner object, use the scanner colours
                wireframeRenderer.material = colourManager.GetColour(scannerColours[index]);
            } else { // Otherwise, use the standard ship colours
                wireframeRenderer.material = colourManager.GetColour(shipColours[index]);
            }
        }
    }
}
