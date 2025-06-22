using UnityEngine;
using UnityEngine.InputSystem;
using static ENet_Channel_Constants;
public class Custom_Input_Manager : MonoBehaviour
{
    // Alex Soca
    // Input manager for Controls from OpenXR

    // References to controller inputs
    public InputActionReference leftTrigger;
    public InputActionReference rightTrigger;
    public InputActionReference rightThumbStick;
    public InputActionReference leftThumbStick;
    public InputActionReference leftThumbStickClick;

    public InputActionReference leftGrab;

    public InputActionReference aButton;
    public InputActionReference bButton;
    public InputActionReference xButton;
    public InputActionReference yButton;

    protected ENet_Client client; // The ENet Client used to communicate with the server


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        client = GameObject.Find("ENetClient").GetComponent<ENet_Client>(); // Find the client from the previous scene

        // Configure keybinds
        leftTrigger.action.started += LeftTriggerPulled;
        leftTrigger.action.canceled += LeftTriggerReleased;

        rightTrigger.action.started += RightTriggerPulled;
        rightTrigger.action.canceled += RightTriggerReleased;


        rightThumbStick.action.performed += RightThumbstickMoved;
        rightThumbStick.action.canceled += RightThumbstickRelease;

        leftThumbStick.action.performed += LeftThumbstickMoved;
        leftThumbStick.action.canceled += LeftThumbstickRelease;

        leftThumbStickClick.action.started += LeftThumbstickClicked;

        leftGrab.action.started += LeftGrabPulled;
        leftGrab.action.canceled += LeftGrabReleased;
        leftGrab.action.performed += LeftGrabPerformed;


        aButton.action.started += AButtonPressed;
        aButton.action.canceled += AButtonReleased;

        bButton.action.started += BButtonPressed;
        bButton.action.canceled += BButtonReleased;

        xButton.action.started += XButtonPressed;
        xButton.action.canceled += XButtonReleased;

        yButton.action.started += YButtonPressed;
        yButton.action.canceled += YButtonReleased;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    protected virtual void AButtonPressed(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void AButtonReleased(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void BButtonPressed(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void BButtonReleased(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void XButtonPressed(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void XButtonReleased(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void YButtonPressed(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void YButtonReleased(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void LeftTriggerPulled(InputAction.CallbackContext context) 
    {
        // Do Something    
    }

    protected virtual void LeftTriggerReleased(InputAction.CallbackContext context)
    {
        // Do Something
    }

    protected virtual void LeftGrabPulled(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void LeftGrabReleased(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void LeftGrabPerformed(InputAction.CallbackContext context) {
        // Do something
    }


    protected virtual void RightTriggerPulled(InputAction.CallbackContext context)
    {
        // Do something
    }

    protected virtual void RightTriggerReleased(InputAction.CallbackContext context) {
        // Do something
    }
    

    protected virtual void LeftThumbstickClicked(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void RightThumbstickMoved(InputAction.CallbackContext context)
    {
        // Do something
    }

    protected virtual void LeftThumbstickMoved(InputAction.CallbackContext context)
    {
        // Do something
    }

    protected virtual void LeftThumbstickRelease(InputAction.CallbackContext context) {
        // Do something
    }

    protected virtual void RightThumbstickRelease(InputAction.CallbackContext context) {
        // Do something
    }
}
