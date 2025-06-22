using UnityEngine;

public class Shield_Controller : MonoBehaviour
{
    // Alex Soca
    // Script for controlling the shield elements of the shield graphic in the critical info panel

    public Renderer frontLeft;
    public Renderer frontRight;
    public Renderer aftLeft;
    public Renderer aftRight;

    private bool updated;
    private int frontShieldStatus;

    private int aftShieldStatus;

    private Colour_Manager colourManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updated = false;
        frontShieldStatus = 255;
        aftShieldStatus = 255;

        colourManager = this.transform.GetComponent<Colour_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(updated) {
            // change material here
            ChangeColours(frontShieldStatus, frontLeft, frontRight);
            ChangeColours(aftShieldStatus, aftLeft, aftRight);

            updated = false;
        }
    }

    public void UpdateValue(int front, int aft) {
        frontShieldStatus = front;
        aftShieldStatus = aft;
        updated = true;
    }

    private void ChangeColours(int status, Renderer left, Renderer right) {
        string targetColour;

        if(status < 5) {
            targetColour = "white"; // Shield (essentially) empty
        } else if(status < 100) {
            targetColour = "red"; // Shield low
        } else if(status < 254) {
            targetColour = "yellow"; // Shield not at maximum
        } else {
            targetColour = "green"; // Shield at maximum
        }

        // Update the material to the new colour
        left.material = colourManager.GetColour(targetColour);
        right.material = colourManager.GetColour(targetColour);
    }
}
