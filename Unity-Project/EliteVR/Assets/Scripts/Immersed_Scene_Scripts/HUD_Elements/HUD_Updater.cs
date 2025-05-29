using UnityEngine;
using System.Collections.Generic;

public class HUD_Updater : MonoBehaviour
{
    // Alex Soca
    // Updates the HUD with information about the Player's ship

    // Indicators in the HUD
    private string[] indicatorNames = {"Speed", "Pitch", "Roll"};

    // List of the indicators
    List<Transform> indicators;

    // List of initial positions of the HUD elements to offset the future transformations by
    List<Vector3> initialPositions;
    private Quaternion initialRotation;

    // Warning Indicator GameObjects
    public GameObject dockingComputerWarning;
    public GameObject hyperDriveWarning;
    public GameObject targetingActiveWarning;
    public GameObject targetLockedWarning;
    public GameObject ecmActiveWarning;

    public Activate_Hangar hangarManager;

    public Missile_Count_Updater missileCountUpdater;

    public Joystick_Controls_Immersed joystickController;
    public Immersed_Local_User_Input_Manager xrController;

    public Hyperspace_Controller hyperspaceController;

    float[] targets; // The target positions of the indicators

    // Status trackers
    bool dockingComputerActive = false;
    bool hyperdriveActive = false;
    bool targetingActive = false;
    bool targetLocked = false;
    bool ecmActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialise vectors
        initialPositions = new List<Vector3>();
        indicators = new List<Transform>();
        targets = new float[3];

        // Store the initial Rotation of the object
        initialRotation = this.transform.rotation;

        // Find the ENet Client script and set this script as the HUD updater
        GameObject.Find("ENetClient").GetComponent<ENet_Client>().HUDUpdater = this;

        // For all indicators
        for(int i = 0; i < 3; i++) {
            indicators.Add(this.transform.Find(indicatorNames[i])); // Get the indicator transform object
            initialPositions.Add(indicators[i].transform.position); // Set the initial position for offsets
            targets[i] = 0; // Initialise the target values
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For all Indicators, Update the position using the target value
        for(int i = 0; i < 3; i++) {
            indicators[i].transform.position = (initialRotation * new Vector3(0, targets[i], 0)) + initialPositions[i];
        }

        // Turn on the HUD docking computer warning if the docking computer is on
        dockingComputerWarning.SetActive(dockingComputerActive);

        // Turn on the HUD docking computer warning if the hyperdrive is active
        hyperDriveWarning.SetActive(hyperdriveActive);
        targetingActiveWarning.SetActive(targetingActive);
        targetLockedWarning.SetActive(targetLocked);
        ecmActiveWarning.SetActive(ecmActive);
    }

    public void UpdateHUDValues(string data) {
        string[] values = data.Split("|");
        int[] translatedValues = new int[18];

        for(int i = 0; i < 18; i++) { // For all values,
            translatedValues[i] = int.Parse(values[i]); // Convert the string to an int
        }

        targets[0] = (((float) translatedValues[0])-20)/400; // Speed (unsigned 0-40)
        targets[1] = ((float) translatedValues[1])/160; // Pitch (signed 0-8)
        targets[2] = ((float) translatedValues[2])/640; // Roll (signed 0-32)

        dockingComputerActive = translatedValues[10] != 0; // Update docking computer status
        
        if(values[11] != "0") { // Check if docked
            hangarManager.EnterHangar();
        } else {
            hangarManager.ExitHangar();
        }

        hyperdriveActive = translatedValues[12] != 0; // Update hyperdrive status

        // Start a 3 second long hyperspace animation when the counter reaches 3
        if(translatedValues[12] == 3) {
            hyperspaceController.EnterHyperspace();
            hyperspaceController.Invoke("ExitHyperspace", 2f);
        }
        
        missileCountUpdater.SetMissileCount(translatedValues[13]); // Update the number of missiles present on ship

        targetingActive = translatedValues[14] != 0; // Update missile targeting system status
        targetLocked = translatedValues[15] != 255; // Update missile target locked status

        ecmActive = translatedValues[16] != 0; // Update ECM status

        // Disable Controllers if dead
        if(translatedValues[17] == 24) {
            joystickController.DisableAxis();
            xrController.DisableAxis();
        }
    }
}
