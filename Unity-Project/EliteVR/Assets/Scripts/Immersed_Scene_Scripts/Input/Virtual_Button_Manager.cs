using UnityEngine;
using UnityEngine.InputSystem;
using static ENet_Channel_Constants;

public class Virtual_Button_Manager : MonoBehaviour
{
    // Alex Soca
    // Input manager for virtual buttons
    private ENet_Client client; // The ENet Client used to communicate with the server
    public Material selected; // The material to set the button to when selected
    
    private Material originalMaterial; // The initial material of the object
    public string buttonCode; // The keycode of the button to send to the server
    private bool serverStreaming = false; // Used to track if the user has requested to stream coordinates from the server
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        client = GameObject.Find("ENetClient").GetComponent<ENet_Client>(); // Find the client from the previous scene
        originalMaterial = gameObject.GetComponent<Renderer>().material; // Store the initial material of the game object
    }

    void RequestCoordinates() {
        if(!serverStreaming) { // If the user has not already requested to stream coordinates,
            serverStreaming = true; // Set the streaming status
            client.SendPacket("Stream Ship Data", 0); // Request the server to begin streaming coordinates
        }
    }

    void PressButton() {
        int temp; // Unused, need for argument of tryparse
        
        if(int.TryParse(buttonCode, out temp)) { // If a number,
            client.SendPacket(buttonCode, PRESS_FUNCTION_KEY); // Press the requested Function Key on the server
        } else { // If a character,
            client.SendPacket(buttonCode, PRESS_KEY); // Press the requested Button on the server
        }
        
        gameObject.GetComponent<Renderer>().material = selected; // Update the material to the selected material
    }

    void PressEnter() {
        client.SendPacket("R", PRESS_RETURN_KEY); // Press the return key on the server

        gameObject.GetComponent<Renderer>().material = selected; // Update the material to the selected material
    }

    void PressNumber() {
        client.SendPacket(buttonCode, PRESS_KEY); // Press the requested number Key on the server

        gameObject.GetComponent<Renderer>().material = selected; // Update the material to the selected material
    }

    void Unselect() {
        gameObject.GetComponent<Renderer>().material = originalMaterial; // Update the material to the original material
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
