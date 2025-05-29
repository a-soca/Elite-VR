using UnityEngine;
using UnityEngine.InputSystem;
using static ENet_Channel_Constants;

public class Home_Virtual_Button_Manager : MonoBehaviour
{
    // Alex Soca
    // Input manager for virtual buttons in the Home Scene
    
    private ENet_Client client; // The ENet Client used to communicate with the server
    public Material selected; // The material to set the button to when selected
    
    private Material originalMaterial; // The initial material of the object
    public string buttonCode; // The keycode of the button to send to the server
    public string arrowDirection; // The direction of the arrow key to send to the server
    public float activationDelay = 0f; // Delay in seconds to wait before turning on the next button
    public GameObject nextInput; // Reference to the next button object
    public Trigger_Game_Start gameStarter; // Script to start the game intro

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        client = GameObject.Find("ENetClient").GetComponent<ENet_Client>(); // Find the client from the previous scene
        originalMaterial = gameObject.GetComponent<Renderer>().material; // Store the initial material of the game object
    }

    void PressButton() {
        client.SendPacket(buttonCode, PRESS_KEY); // Press the requested Button on the server
        gameObject.GetComponent<Renderer>().material = selected; // Update the material to the selected material
    }
    
    void PressArrow() {
        client.SendPacket(arrowDirection, PRESS_ARROW_KEY); // Press the requested Arrow on the server
        gameObject.GetComponent<Renderer>().material = selected; // Update the material to the selected material
    }

    void PressReturn() {
        client.SendPacket("R", PRESS_RETURN_KEY);
        gameObject.GetComponent<Renderer>().material = selected; // Update the material to the selected material
    }

    void Unselect() {
        gameObject.GetComponent<Renderer>().material = originalMaterial; // Update the material to the original material
    }

    void AllowNextInput() { // Activates the next input after the provided delay
        Invoke("ActivateNextInput", activationDelay);
        
    }

    void ActivateNextInput() { // Activates next input immediately
        nextInput.SetActive(true);
    }

    void Disable() { // Turns this object off
        this.gameObject.SetActive(false);
    }

    void StartGame() { // Activates the start sequence
        gameStarter.Invoke("StartSequence", 8.5f); // asynchronously Call StartSequence function after 8 seconds
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
