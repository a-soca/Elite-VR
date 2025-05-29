using UnityEngine;
using System.Collections;
using System;

public class Space_Dust_Updater : MonoBehaviour
{
    // Alex Soca
    // Controls the speed and orientation of space dust based on the movement of the players ship

    private float speed = 0;
    private float pitch = 0;
    private float roll = 0;

    private ParticleSystem particles; // The space dust particles

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the ENet Client script and set this script as the space dust manager
        GameObject.Find("ENetClient").GetComponent<ENet_Client>().spaceDustUpdater = this;

        particles = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var velocity = particles.velocityOverLifetime;
        
        velocity.z = -speed; // Update the speed
        velocity.orbitalX = pitch; // Update the pitch
        velocity.orbitalZ = -roll; // Update the roll
    }

    public void UpdateSpaceDust(string data) {
        string[] values = data.Split("|");
        int[] translatedValues = new int[3];

        for(int i = 0; i < 3; i++) { // For all values,
            translatedValues[i] = int.Parse(values[i]); // Convert the string to an int
        }

        speed = ((float) translatedValues[0])*2; // Speed
        pitch = ((float) translatedValues[1])/10; // Pitch
        roll = ((float) translatedValues[2])/16; // Roll
    }
}
