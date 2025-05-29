using UnityEngine;
using UnityEngine.InputSystem;
using static ENet_Channel_Constants;

public class Home_Local_User_Input_Manager : Custom_Input_Manager
{
    // Alex Soca
    // Local Input handler for the Home Scene

    public bool gameLoaded = false; // Used to check if the game disk has been inserted
    public Trigger_Game_Start gameStartScript; // The script used to trigger the next scene

    private bool starting = false; // Used to track if the user has already requested to start the game

    protected override void AButtonPressed(InputAction.CallbackContext context) {
        NextScene();
    }

    protected override void BButtonPressed(InputAction.CallbackContext context) {
        NextScene();
    }

    protected override void RightTriggerPulled(InputAction.CallbackContext context) {
        NextScene();
    }

    protected override void XButtonPressed(InputAction.CallbackContext context) {
        NextScene();
    }

    protected override void YButtonPressed(InputAction.CallbackContext context) {
        NextScene();
    }

    protected override void LeftTriggerPulled(InputAction.CallbackContext context) {
        NextScene();
    }

    void NextScene() {
        if(gameLoaded && !starting) { // If the game has been loaded and the user has not already attempted to start the next scene
            starting = true; // Set flag to prevent double function trigger
            gameStartScript.Invoke("EnterImmersed", 0f); // Trigger the next scene function after 0 seconds
        }
    }
}
