using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bar_Controller : MonoBehaviour
{
    // Alex Soca
    // Controls information bars to make them display a percentage value visually
    public float maxVal = 255f; // The maximum value of the bar (Defaults to 255)

    // Default positional information about the bar used to change its scale
    private Vector3 initialScale; 
    private Vector3 initialPosition;
    protected Vector3 direction; // Determines which direction to scale the bar

    private Transform bar;

    protected float percentage = 1; // The default percentage value (100%)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetBars();

        GetPositions();

        direction = new Vector3(0.2f, 0f, 0f); // The direction in which the bar should shrink
    }

    protected virtual void GetBars() {
        bar = this.gameObject.transform; // Get the transform of the bar
    }

    protected virtual void GetPositions() {
        // Automatically find and store the bar position and scale
        initialPosition = gameObject.transform.position;
        initialScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScale();
    }

    protected virtual void UpdateScale() {
        // Scale the bar to the correct length based on percentage filled
        bar.transform.localScale = initialScale + (direction * (percentage-1f));

        // Offset the position to keep the bar left justified as it changes scale
        bar.transform.position = initialPosition + (direction * (percentage-1f))/2;
    }

    // Calculates and sets the percentage value of the bar
    public void UpdateValue(int value) {
        percentage = ((float) value)/maxVal;
        // Transforms cannot be modified outside of the main thread so a 
        // variable must be set and read by the update function
    }
}
