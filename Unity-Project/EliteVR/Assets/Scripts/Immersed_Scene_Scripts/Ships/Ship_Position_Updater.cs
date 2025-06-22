using UnityEngine;
using System;

public class Ship_Position_Updater : MonoBehaviour
{
    // Alex Soca
    // Script to set the position of a game object from a coordinate packet
    private Vector3 positionalVelocity = Vector3.zero; // Initialise the positional velocity to 0
    private Vector3 rotationalVelocity = Vector3.zero; // Initialise the rotational velocity to 0
    private Vector3 nextCoordinate; // Store the target coordinate
    private Vector3 lastCoordinate = Vector3.zero; // Stores the previous coordinate
    private Quaternion nextOrientation; // Store the target orientation

    // Ship Flags
    public bool active = false;
    private bool exploding = false;
    private bool explosionTrigger = false;
    private bool firingLasers = false;
    public bool scannerVisibility = false;

    public bool explosive = true;
    private bool deactivated = true;

    public int interpolationSkipThreshold = 200;

    // Utilities
    private Ship_Model_Updater modelUpdater; // Used to change the model to match the ship ID
    
    public void UpdatePosition(string inputString) {
        // The input should be passed in the format:
        // Format: [A]|[B]|[C]|[D]|[E]|[X],[Y],[Z]|[NX],[NY],[NZ]|[RX],[RY],[RZ]|[SX],[SY],[SZ]
        // [A] = Ship ID
        // [B] = Visible on Scanner
        // [C] = Exploding
        // [D] = Firing Lasers
        // [E] = Killed
        // [X] = X coordinate
        // [Y] = Y coordinate
        // [Z] = Z coordinate
        // [NX] = Nose X Vector
        // [NY] = Nose Y Vector
        // [NZ] = Nose Z Vector
        // [RX] = Roof X Vector
        // [RY] = Roof Y Vector
        // [RZ] = Roof Z Vector
        // [SX] = Side X Vector
        // [SY] = Side Y Vector
        // [SZ] = Side Z Vector
        // Example: 2|1|0|0|0|-100,20,400|-20,2,6|14,20,-50|90,45,-10` = Ship with ID 2 is visible on scanner, not exploding, firing lasers or killed and at position (-100, 20, 400) with nose vector (-20, 2, 6), roof vector (14, 20, -50) and side vector (90, 45, -10)
        // Each ship block is delimited with a \n char, therefore by splitting the string by newline we can retrieve the ship slot by the index in the split array

        string[] segments = inputString.Split('|'); // Split the string into 5 components delimited by vertical bar
        
        // Safety net
        if(segments.Length != 9 || segments == null) {
            return;
        }

        modelUpdater.SetModelByID(int.Parse(segments[0])); // Set the model based on the ship ID

        scannerVisibility = segments[1] != "0"; // If not 0, the ship is visible

        if(segments[4] == "1" || exploding) { // If the ship has been killed,
            active = false; // set it to be inactive
        }

        bool previousExplodingState = exploding; // Store the previous explosion state
        exploding = segments[2] != "0"; // Find the current explosion state
        
        if(previousExplodingState != exploding && exploding) { // If the explosion state has changed and it is true,
            explosionTrigger = true; // Trigger the explosion
        }

        firingLasers = segments[3] != "0" && active;

        UpdateCoordinates(segments[5]); // Update the ship position
        UpdateOrientation(segments[6], segments[7], segments[8]); // Update the ship orientation
    }

    private Vector3 ExtractVector3FromString(string vectorString) {
        string[] xyzString = vectorString.Split(','); // Split the coordinate string into 3 components delimited by comma

        int[] xyz = new int[3]; // Create an int array for the coordinates

        for(int i = 0; i < 3; i++) { // For all 3 dimensions,
            xyz[i] = int.Parse(xyzString[i]); // Convert the string to an int
        }

        return new Vector3(xyz[0], xyz[1], xyz[2]); // Convert to Vector3
    }

    private void UpdateCoordinates(string coordinateString) {
        nextCoordinate = ExtractVector3FromString(coordinateString); // Update the target position
    }

    private void UpdateOrientation(string noseVectorString, string roofVectorString, string sideVectorString) {
        Vector3 noseVector = ExtractVector3FromString(noseVectorString);
        Vector3 roofVector = ExtractVector3FromString(roofVectorString);
        Vector3 sideVector = ExtractVector3FromString(sideVectorString);

        var matrix = new Matrix4x4();
        matrix.SetColumn(0, sideVector);
        matrix.SetColumn(1, roofVector);
        matrix.SetColumn(2, noseVector);

        matrix.SetRow(3, new Vector4(0, 0, 0, 1));
        matrix.SetColumn(3, new Vector4(0, 0, 0, 1));

        nextOrientation = matrix.rotation;
        nextOrientation *= Quaternion.Euler(90, 0, 0); // For Space Station

    }

    public void Deactivate() {
        deactivated = true;
    }

    public void Activate() {
        deactivated = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        modelUpdater = this.transform.GetComponent<Ship_Model_Updater>();

        nextCoordinate = transform.position; // Initialise position variable
        nextOrientation = transform.rotation; // Initalise rotation variable
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object the script is attached to
        // Using delta time to keep interpolation speed constant between frame rates (400+ on pc, 70 in VR)
        // Interpolation is used to fill in the gaps between the jumps in coordinates due to the low positional resolution of the emulator
        
        float differenceInMagnitude = Mathf.Abs(transform.position.magnitude-nextCoordinate.magnitude);
        if(differenceInMagnitude > interpolationSkipThreshold) {
            transform.position = nextCoordinate;
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, nextCoordinate, ref positionalVelocity, 0.1f*(1f-Time.deltaTime)); 
        }

       // transform.rotation = nextOrientation; 
        transform.rotation = Quaternion.Lerp(transform.rotation, nextOrientation, Time.time); // update the orientation of the ship

        active = !exploding && !deactivated;
        
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
            mr.enabled = active; // Set object visibility based on if ship is killed or not
        } 
       
        FireLasers(firingLasers);

        if(explosionTrigger) { Explode(); }
    }

    private void FireLasers(bool status) {
        var lasers = this.transform.Find("Lasers");

        if(exploding || !active) {
            status = false;
        }

        if(lasers != null) {
            lasers.gameObject.SetActive(status);
        }
    }

    private void Explode() {
        if(!explosive) { // If this object is a planet or space station (slot 1 or 2), do not explode
            return;
        }

        explosionTrigger = false;
        var explosionParent = this.transform.Find("ExplosionGenerator");
        this.GetComponent<Explosion_Particle_Updater>().ExplosionTriggered();

        if(explosionParent != null) {
            var burst1 = explosionParent.transform.Find("Red");

            burst1.gameObject.GetComponent<ParticleSystem>().Emit(500);
            Invoke("SecondExplosion", 0.5f); // asynchronously emit the second explosion after 0.5 seconds
        }
    }

    private void SecondExplosion() {
        var burst2 = this.transform.Find("ExplosionGenerator").transform.Find("SecondRed");
        burst2.gameObject.GetComponent<ParticleSystem>().Emit(500);
    }
} 