using UnityEngine;
using UnityEngine.InputSystem;
using static ENet_Channel_Constants;

public class Immersed_Local_User_Input_Manager : Custom_Input_Manager
{
    // Alex Soca
    // Local Input Handling for the Immersed Scene
    float stickDeadzone = 0.5f; // The radius of the stick to ignore input within (sensitivity)

    // Variables to track input holds
    private bool holdingLXAxis = false;
    private bool holdingLYAxis = false;

    public GameObject cockpit;

    bool targeting = false;

    bool axisDisabled = false;

    protected override void AButtonPressed(InputAction.CallbackContext context) {
        client.SendPacket("y", PRESS_KEY); // Press the Y key
    }
    protected override void BButtonPressed(InputAction.CallbackContext context) {
        client.SendPacket("n", PRESS_KEY); // Press the N key
    }

    protected override void XButtonPressed(InputAction.CallbackContext context) {
        client.SendPacket("/", HOLD_KEY); // Hold the ? key
    }

    protected override void XButtonReleased(InputAction.CallbackContext context) {
        client.SendPacket("/", RELEASE_KEY); // Release the ? key
    }

    protected override void YButtonPressed(InputAction.CallbackContext context) {
        client.SendPacket(" ", HOLD_KEY); // Hold the space bar
    }

    protected override void YButtonReleased(InputAction.CallbackContext context) {
        client.SendPacket(" ", RELEASE_KEY); // Release the space bar
    }

    protected override void RightTriggerPulled(InputAction.CallbackContext context) {
        client.SendPacket("a", HOLD_KEY); // Hold the A key
        FireLasers();
    }

    protected override void RightTriggerReleased(InputAction.CallbackContext context) {
        client.SendPacket("a", RELEASE_KEY); // Release the A key
        TurnOffLasers();
    }
    

    protected override void LeftThumbstickClicked(InputAction.CallbackContext context) {
        client.SendPacket("o", PRESS_KEY); // Press the O key
    }

    protected override void LeftTriggerPulled(InputAction.CallbackContext context) {
        client.SendPacket("m", PRESS_KEY); // Press the m key
    }

    protected override void LeftGrabPulled(InputAction.CallbackContext context) {
        if(targeting) {
            client.SendPacket("u", PRESS_KEY); // Press the u key
            targeting = false;
        } else {
            client.SendPacket("t", PRESS_KEY); // Press the t key
            targeting = true;
        }
    }

    protected override void RightThumbstickMoved(InputAction.CallbackContext context) {
        if(axisDisabled) {
            return;
        }

        // Gets the value of the Vector2 read from the thumbstick position
        UnityEngine.Vector2 extractedVector = context.ReadValue<UnityEngine.Vector2>(); 

        int xVal = (int) -((extractedVector.x*127) + 128);
        int yVal = (int) ((extractedVector.y*127) + 128);

        client.SendPacket(xVal.ToString(), ANALOG_X);
        client.SendPacket(yVal.ToString(), ANALOG_Y);
    }

    protected override void LeftThumbstickMoved(InputAction.CallbackContext context) {
        // Gets the value of the Vector2 read from the thumbstick position
        UnityEngine.Vector2 extractedVector = context.ReadValue<UnityEngine.Vector2>(); 

        if(extractedVector.x > stickDeadzone) {
            if(!holdingLXAxis) {
                holdingLXAxis = true;
                client.SendPacket("R", HOLD_ARROW_KEY); // Thumbstick Right
            }
        } else if(extractedVector.x < -stickDeadzone) {
            if(!holdingLXAxis) {
                holdingLXAxis = true;
                client.SendPacket("L", HOLD_ARROW_KEY); // Thumbstick Left
            }
        } else if(holdingLXAxis) {
            holdingLXAxis = false; // Thumbstick in deadzone of x axis
            client.SendPacket("R", RELEASE_ARROW_KEY);
            client.SendPacket("L", RELEASE_ARROW_KEY);
        }

        if(extractedVector.y > stickDeadzone) {
            if(!holdingLYAxis) {
                holdingLYAxis = true;
                client.SendPacket("U", HOLD_ARROW_KEY); // Thumbstick Up
            }
        } else if(extractedVector.y < -stickDeadzone) {
            if(!holdingLYAxis) {
                holdingLYAxis = true;
                client.SendPacket("D", HOLD_ARROW_KEY); // Thumbstick Down
            }
        } else if(holdingLYAxis) {
            holdingLYAxis = false; // Thumbstick in deadzone of y axis
            client.SendPacket("U", RELEASE_ARROW_KEY);
            client.SendPacket("D", RELEASE_ARROW_KEY);
        }
    }

    protected override void LeftThumbstickRelease(InputAction.CallbackContext context) {
        client.SendPacket("U", RELEASE_ARROW_KEY);
        client.SendPacket("D", RELEASE_ARROW_KEY);
        client.SendPacket("R", RELEASE_ARROW_KEY);
        client.SendPacket("L", RELEASE_ARROW_KEY);
    }

    protected override void RightThumbstickRelease(InputAction.CallbackContext context) {
        client.SendPacket(",", RELEASE_KEY);
        client.SendPacket(".", RELEASE_KEY);
        client.SendPacket("s", RELEASE_KEY);
        client.SendPacket("x", RELEASE_KEY);
    }

    private void FireLasers() {
        cockpit.transform.Find("Lasers").gameObject.SetActive(true);
    }

    private void TurnOffLasers() {
        cockpit.transform.Find("Lasers").gameObject.SetActive(false);
    }

    public void DisableAxis() {
        axisDisabled = true;
        Invoke("EnableAxis", 5f);
    }

    public void EnableAxis() {
        axisDisabled = false;
    }
}
