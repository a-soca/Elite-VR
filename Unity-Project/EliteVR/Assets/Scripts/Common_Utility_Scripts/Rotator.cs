using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Alex Soca
    // Basic rotation script to constantly rotate the object the script is attached to
    public float rotateSpeed; // The speed to rotate the object
    public Vector3 rotationDirection = new Vector3(); // The axis to rotate

    // Update is called once per frame
    void Update()
    {
        // Use delta time to rotate the object consistently across frames
        transform.Rotate(rotateSpeed * rotationDirection * Time.deltaTime); 
    }
}
