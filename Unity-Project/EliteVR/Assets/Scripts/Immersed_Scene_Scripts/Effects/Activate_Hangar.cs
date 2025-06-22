using UnityEngine;

public class Activate_Hangar : MonoBehaviour
{
    // Alex Soca
    // Script to update gameobject visibilty when entering the hangar

    GameObject spaceDust;
    GameObject shipContainer;
    GameObject hangar;

    GameObject keypad;
    GameObject hangarFunctions;

    public Hyperspace_Controller hyperspaceController;

    bool inHangar = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spaceDust = GameObject.Find("GlobalParticles");
        shipContainer = GameObject.Find("ShipContainer");
        hangar = GameObject.Find("Hangar");

        hangarFunctions = GameObject.Find("HangarFunctions");
        keypad = GameObject.Find("KeyPadButtons");

        EnterHangar(); // Set to in hangar
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterHangar() {
        if(!inHangar) {
            hangarFunctions.SetActive(true);
            keypad.SetActive(true);
            spaceDust.SetActive(false);
            shipContainer.SetActive(false);
            hangar.SetActive(true);
            inHangar = true;
        }
    }

    public void ExitHangar() {
        if(inHangar) {
            hyperspaceController.EnterHyperspace();
            hyperspaceController.Invoke("ExitHyperspace", 0.25f);
            hangarFunctions.SetActive(false);
            keypad.SetActive(false);
            spaceDust.SetActive(true);
            shipContainer.SetActive(true);
            hangar.SetActive(false);
            inHangar = false;
        }
    }
}
