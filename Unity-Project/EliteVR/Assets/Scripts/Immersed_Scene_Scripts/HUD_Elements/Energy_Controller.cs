using UnityEngine;

public class Energy_Controller : Bar_Controller
{
    // Alex Soca
    // A more sophisticated version of the Bar_Controller for managing the 4 energy bars and events triggered by low health

    private Transform[] bars = new Transform[4]; // Stores the 4 energy bars

    // Stores the initial positions of each bar
    private Vector3[] initialScales = new Vector3[4];
    private Vector3[] initialPositions = new Vector3[4];

    public GameObject lowHealthSparks; // Visual cue for low health
    public GameObject veryLowHealthSparks; // Visual cue for very low health
    public Light emergencyLight;

    public GameObject lowEnergyText; // Text warning for low health

    protected override void GetBars() {
        // Automatically find and store the energy bars
        for(int i = 0; i < 4; i++) {
            bars[i] = transform.Find("Energy" + (i+1)).gameObject.transform;
        }
    }

    protected override void GetPositions() {
        // Automatically find and store the positions of the energy bars
        for(int i = 0; i < 4; i++) {
            initialPositions[i] = bars[i].gameObject.transform.position;
            initialScales[i] = bars[i].gameObject.transform.localScale;
        }
    }

    protected override void UpdateScale() {
        TriggerEvents(); // Trigger any events based on health level

        float workingPercentage = percentage; // Store a temporary copy of the percentage
        float remainder = 0f; // Initialise the remainder

        // For each of the 4 quartiles (bars)
        for(int i = 0; i < 4; i++) {
            float quartile = 0.25f*(3-i); // Get the quartile percentage

            if(workingPercentage >= quartile) { // If the working percentage is above the quartile,
                // Find what portion of the bar has been depleted
                remainder = workingPercentage - quartile;

                // Scale the bar to the correct length based on percentage filled
                bars[i].transform.localScale = initialScales[i] + (direction * (remainder-0.25f)*4);

                // Offset the position to keep the bar left justified as it changes scale
                bars[i].transform.position = initialPositions[i] + (direction * (remainder-0.25f)*4)/2;

                // Subtract the remainder from working percentage
                workingPercentage -= remainder;
            } else { // Otherwise, the bar is empty
                // Empty bar
                // Scale the bar to 0%
                bars[i].transform.localScale = initialScales[i] + (direction * -1f);

                // Offset the position to keep the bar left justified as it changes scale
                bars[i].transform.position = initialPositions[i] + (direction * -1f)/2;
            }
        }
    }

    void TriggerEvents() {
        // Turn on the sparks if energy is less than 0.75f
        lowHealthSparks.SetActive(percentage < 0.75f);
        veryLowHealthSparks.SetActive(percentage < 0.4f);

        // Turn on the emergency lights
        if(percentage < 0.4f) {
            emergencyLight.color = Color.red;
            emergencyLight.enabled = (Time.fixedTime % 0.5f < 0.25f);
        } else {
            emergencyLight.enabled = true;
            emergencyLight.color = Color.white;
        }
        

        // Show the low energy warning
        lowEnergyText.SetActive(percentage < 0.2f);
    }
}
