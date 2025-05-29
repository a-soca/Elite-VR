using UnityEngine;

public class Respawnable : MonoBehaviour
{
    // Alex Soca
    // Script to respawn game objects if they exceed user specifiable bounds


    // The minimum and maximum values for each direction
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;
    public float zmin;
    public float zmax;

    private Vector3 defaultPosition; // The original position of the object

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultPosition = transform.position; // Store the initial position
    }

    // Update is called once per frame
    void Update()
    {
        // If the position excedes the limits in any direction,
        if(transform.position.x < xmin 
        || transform.position.x > xmax
        || transform.position.y < ymin
        || transform.position.y > ymax
        || transform.position.z < zmin
        || transform.position.z > zmax) {
            Respawn(); // Respawn
        }
    }

    void Respawn() {
        transform.position = defaultPosition; // Set the object position back to the initial position
    }
}
