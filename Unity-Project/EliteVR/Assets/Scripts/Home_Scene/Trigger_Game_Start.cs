using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Trigger_Game_Start : MonoBehaviour
{
    // Alex Soca
    // Script to handle the game start and transition to the immersed scene
    public ENet_Client client; // The ENet Client used to communicate with the server
    public Home_Local_User_Input_Manager inputManager; // The script used to interpret user inputs
    public GameObject eliteLogo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.name == "DiskReader") {
            // Disable the hint label
            GameObject.Find("DiskReader").gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Invoke("EnableVirtualButtons", 4f); // Wait 4 seconds before enabling the virtual buttons
            
            client.SendPacket("Load Elite", 0); // Send a request to the server to load the game
            gameObject.SetActive(false); // Make invisible and prevent further interaction
        }
    }

    void StartSequence() {
        inputManager.gameLoaded = true; // Tell the input manager that the game has been loaded

        eliteLogo.SetActive(true); // Turn on the elite logo to allow transport to the next scene
        
        // Destroy environment objects
        Destroy(GameObject.Find("Room"));
        Destroy(GameObject.Find("Desk"));

        // Start floating objects in front of player
        FloatObject("Monitor", new Vector3(-2.5f, 2.5f, 2.5f), new Vector3(0.75f, 0.75f, 0.75f));
        FloatObject("Computer", new Vector3(7.5f, 1f, 5f), new Vector3(-0.5f, -0.5f, -0.5f));

        FloatObject("DeskLamp", new Vector3(-10f, 5f, -1f), new Vector3(-10f, 5f, -1f));
        FloatObject("DiskReader", new Vector3(5f, 5f, 1f), new Vector3(5f, 5f, 1f));
    }

    void FloatObject(string name, Vector3 Direction, Vector3 Torque) {
        Transform tf = GameObject.Find(name).transform; // Get the transform component of the object
        Rigidbody rb = tf.GetComponent<Rigidbody>(); // Get the rigidbody component of the object

        rb.constraints = RigidbodyConstraints.None; // Remove constraints to allow free movement
        rb.AddForce(Direction); // Add the specified force
        rb.AddTorque(Torque); // Rotate the object
    }

    public void EnterImmersed() {
        if(inputManager.gameLoaded) {
            SceneManager.LoadScene("Assets/Scenes/ImmersedScene.unity", LoadSceneMode.Single); // Change to the immersed scene
        }
    }

    void EnableVirtualButtons() {
        GameObject.Find("VirtualButtons").gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
