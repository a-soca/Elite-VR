using UnityEngine;

public class Make_Transparent : MonoBehaviour
{
    // Alex Soca
    // Makes the material of the object the script is attached to transparent using a provided alpha value
    // Note: material must have the transparent property
    public float alpha = 0.01f; // Default Alpha

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color color = GetComponent<Renderer>().material.color; // Get the colour of the material
        color.a = alpha; // Set the alpha value
        GetComponent<Renderer>().material.color = color; // Update the colour of the material
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
