using UnityEngine;

public class Dont_Destroy_On_Load : MonoBehaviour
{
    // Alex Soca
    // Script to prevent the attached game object from being destroyed when a new scene is loaded
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Tell unity to keep this object in the next scene
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
