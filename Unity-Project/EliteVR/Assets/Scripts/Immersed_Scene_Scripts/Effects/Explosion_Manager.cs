using UnityEngine;
using System.Collections.Generic;

public class Explosion_Manager : MonoBehaviour
{
    // Alex Soca
    // The manager for all Explosion Particle Updaters
    Explosion_Particle_Updater[] updaters; // Array of updaters for the individual explosion sources

    // Variables to store the player's movement to translate the explosion position after emission
    private float speed = 0.0f;
    private float pitch = 0.0f;
    private float roll = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the ENet Client script and set this script as the explosion manager
        GameObject.Find("ENetClient").GetComponent<ENet_Client>().explosionManager = this;

        // Find all the explosion particle updaters
        updaters = this.GetComponentsInChildren<Explosion_Particle_Updater>();    
    }

    // Update is called once per frame
    void Update()
    {
        // Translate the explosion by the players movement
        foreach(Explosion_Particle_Updater u in updaters) {
            u.UpdateParticles(speed, pitch, roll);
        }
    }

    public void UpdateMovementInformation(string data) {
        string[] values = data.Split("|"); // Split the packet into speed, pitch and roll
        int[] translatedValues = new int[3]; // Initialise an array to store the parsed values

        for(int i = 0; i < 3; i++) { // For all values,
            translatedValues[i] = int.Parse(values[i]); // Convert the string to an int
        }

        speed = ((float) translatedValues[0])*2; // Speed
        pitch = ((float) translatedValues[1])/10; // Pitch
        roll = ((float) translatedValues[2])/16; // Roll
    }
}
