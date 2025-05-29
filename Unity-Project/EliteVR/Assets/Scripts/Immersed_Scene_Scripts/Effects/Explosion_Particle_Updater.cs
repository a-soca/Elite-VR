using UnityEngine;
using System.Collections.Generic;

public class Explosion_Particle_Updater : MonoBehaviour
{
    // Alex Soca
    // Script to update explosion particle position after emission
    public Vector3 explosionPosition = Vector3.zero; // Initialise the explosion position to the origin
    ParticleSystem[] particleSystems; // Array to store the particle systems which create the explosions

    // Variables to store the player's movement
    private float speed = 0f;
    private float pitch = 0f;
    private float roll = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the explosion generator attached to this object and get the particle systems from its children
        particleSystems = this.transform.Find("ExplosionGenerator").GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // For every particle system,
        foreach(ParticleSystem p in particleSystems) {
            var velocity = p.velocityOverLifetime; // Get the velocity over lifetime module
            
            // Translate the origin of rotation to the players position
            velocity.orbitalOffsetX = -explosionPosition.x;
            velocity.orbitalOffsetY = -explosionPosition.y;
            velocity.orbitalOffsetZ = -explosionPosition.z;

            velocity.z = -speed*4; // Update the speed
            velocity.orbitalX = pitch; // Update the pitch
            velocity.orbitalZ = -roll; // Update the roll
        }
    }

    // Sets the speed, pitch and roll used in the game loop when updating particles
    public void UpdateParticles(float speed, float pitch, float roll) {
        this.speed = speed;
        this.pitch = pitch;
        this.roll = roll;
    }

    public void ExplosionTriggered() {
        // Zero the euler angles so that the pitch, roll and translation matches the player's orientation
        this.transform.Find("ExplosionGenerator").eulerAngles = Vector3.zero;
        explosionPosition = this.transform.position;
    }
}
