using UnityEngine;
using System.Collections.Generic;

public class Missile_Count_Updater : MonoBehaviour
{
    // Alex Soca
    // Script to update the missile count indicator
    List<GameObject> indicators; // A list of the indicator objects

    // The number of missiles loaded
    int numMissiles = 4;
    int newNumMissiles = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        indicators = new List<GameObject>(4);
        // Automatically get the indicator gameobjects
        for(int i = 0; i < 4; i++) {
            indicators.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the number of missiles has changed since last frame.
        if(newNumMissiles != numMissiles) {
            numMissiles = newNumMissiles; // Update the number of missiles

            // For the number of missiles expended,
            for(int i = 0; i < 4 - newNumMissiles; i++) {
                indicators[i].SetActive(false); // Disable the indicator
            }

            // For the number of missiles present,
            for(int i = 4 - newNumMissiles; i < 4; i++) {
                indicators[i].SetActive(true); // Enable the indicator
            }
        }
    }

    public void SetMissileCount(int count) {
        newNumMissiles = count;
    }
}
