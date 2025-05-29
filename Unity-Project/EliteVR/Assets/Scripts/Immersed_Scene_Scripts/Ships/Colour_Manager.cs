using UnityEngine;

public class Colour_Manager : MonoBehaviour
{
    // Alex Soca
    // Script to manage material colour requests
    public Material cyan;
    public Material blue;
    public Material red;
    public Material magenta;
    public Material yellow;
    public Material green;
    public Material white;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Return Material
    public Material GetColour(string colour) {
        switch(colour) {
            case "cyan":
                return cyan;
            case "blue":
                return blue;
            case "red":
                return red;
            case "magenta":
                return magenta;
            case "yellow":
                return yellow;
            case "green":
                return green;
            default:
                return white;
        }
    }
}
