using UnityEngine;
using UnityEngine.InputSystem;
using static ENet_Channel_Constants;

public class Joystick_Controls_Immersed : MonoBehaviour
{
    // Alex Soca
    // Local Input Handling for the Immersed Scene

    public InputActionReference mainAxis;
    public InputActionReference trigger;

    public InputActionReference leftEncoderUp;
    public InputActionReference leftEncoderDown;

    public InputActionReference cursorX;
    public InputActionReference cursorY;

    public InputActionReference thumbHatPress;
    public InputActionReference redButton;
    public InputActionReference secondaryTriggerPull;
    public InputActionReference secondaryTriggerPush;

    public GameObject cockpit;

    bool axisDisabled = false;

    private ENet_Client client;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        client = GameObject.Find("ENetClient").GetComponent<ENet_Client>(); // Find the client from the previous scene

        // Configure keybinds
        trigger.action.started += TriggerPulled;
        trigger.action.canceled += TriggerReleased;

        leftEncoderUp.action.started += leftEncoderUpStarted;
        leftEncoderUp.action.canceled += leftEncoderUpStopped;

        leftEncoderDown.action.started += leftEncoderDownStarted;
        leftEncoderDown.action.canceled += leftEncoderDownStopped;

        cursorX.action.started += cursorXMoved;
        cursorX.action.canceled += cursorXReleased;

        cursorY.action.started += cursorYMoved;
        cursorY.action.canceled += cursorYReleased;

        thumbHatPress.action.started += ThumbHatPress;
        redButton.action.started += RedButtonPress;
        secondaryTriggerPull.action.started += SecondaryTriggerPull;
        secondaryTriggerPush.action.started += SecondaryTriggerPush;
    }

    protected void ThumbHatPress(InputAction.CallbackContext context) {
        client.SendPacket("e", PRESS_KEY); // press the e key
    }

    protected void RedButtonPress(InputAction.CallbackContext context) {
        client.SendPacket("m", PRESS_KEY); // press the m key
    }

    protected void SecondaryTriggerPull(InputAction.CallbackContext context) {
        client.SendPacket("t", PRESS_KEY); // press the t key
    }
    
    protected void SecondaryTriggerPush(InputAction.CallbackContext context) {
        client.SendPacket("u", PRESS_KEY); // press the u key
    }
    

    // Update is called once per frame
    void Update()
    {
        // Every 15 frames, poll the axis
        if(Time.frameCount % 15 == 0) {
            var value = mainAxis.action.ReadValue<Vector2>(); // Extract the axis position

            // If not centred
            if(value != Vector2.zero) {
                MoveAxis(value); // Move the pitch and roll to the position requested
            }
        }
    }

    protected void TriggerPulled(InputAction.CallbackContext context) {
        client.SendPacket("a", HOLD_KEY); // Hold the A key
        FireLasers();
    }

    protected void TriggerReleased(InputAction.CallbackContext context) {
        client.SendPacket("a", RELEASE_KEY); // Release the A key
        TurnOffLasers();
    }

    private void MoveAxis(Vector2 input) {
        if(axisDisabled) {
            return;
        }

        // Temporary fix for buttons triggering axis
        if(CheckMisInput(input)) {
            return;
        }
            

        int xVal = (int) -((input.x*127) + 128);
        int yVal = (int) ((input.y*127) + 128);

        client.SendPacket(xVal.ToString(), ANALOG_X);
        client.SendPacket(yVal.ToString(), ANALOG_Y);
    }

    private bool CheckMisInput(Vector2 input) {
        float x = input.x;
        float y = input.y;

        if(Mathf.Approximately(x, -0.7071068f) && Mathf.Approximately(y, 0.7071068f)) {
            return true;
        } else if(Mathf.Approximately(x, -0.5999687f) && Mathf.Approximately(y, 0.8000234f)) {
            return true;
        } else if(Mathf.Approximately(x, -0.5299431f) && Mathf.Approximately(y, 0.8480332f)) {
            return true;
        } else if(Mathf.Approximately(x, -0.7064151f) && Mathf.Approximately(y, 0.7077978f)) {
            return true;
        } else if(Mathf.Approximately(x, -0.7057213f) && Mathf.Approximately(y, 0.7084895f)) {
            return true;
        }

        return false;
    }

    private void cursorXMoved(InputAction.CallbackContext context) {
        var value = context.ReadValue<float>();
        if(value < 0) {
            client.SendPacket("L", HOLD_ARROW_KEY);
        } else {
            client.SendPacket("R", HOLD_ARROW_KEY);
        }
    }

    private void cursorXReleased(InputAction.CallbackContext context) {
        client.SendPacket("R", RELEASE_ARROW_KEY);
        client.SendPacket("L", RELEASE_ARROW_KEY);
    }

    private void cursorYMoved(InputAction.CallbackContext context) {
        var value = context.ReadValue<float>();
        if(value < 0) {
            client.SendPacket("U", HOLD_ARROW_KEY);
        } else {
            client.SendPacket("D", HOLD_ARROW_KEY);
        }
    }

    private void cursorYReleased(InputAction.CallbackContext context) {
        client.SendPacket("U", RELEASE_ARROW_KEY);
        client.SendPacket("D", RELEASE_ARROW_KEY);
    }

    protected void leftEncoderUpStarted(InputAction.CallbackContext context) {
        client.SendPacket(" ", HOLD_KEY); // Hold the space bar
    }

    protected void leftEncoderUpStopped(InputAction.CallbackContext context) {
        client.SendPacket(" ", RELEASE_KEY); // Release the space bar
    }

    protected void leftEncoderDownStarted(InputAction.CallbackContext context) {
        client.SendPacket("/", HOLD_KEY); // Hold the ? key
    }

    protected void leftEncoderDownStopped(InputAction.CallbackContext context) {
        client.SendPacket("/", RELEASE_KEY); // Release the ? key
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
