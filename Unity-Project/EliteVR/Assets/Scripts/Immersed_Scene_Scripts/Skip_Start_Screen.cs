using UnityEngine;
using static ENet_Channel_Constants;

public class Skip_Start_Screen : MonoBehaviour
{
    // Alex Soca
    // Script to skip the start screen of the game on load
    ENet_Client client;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        client = GameObject.Find("ENetClient").GetComponent<ENet_Client>(); // Find the client from the previous scene
    }

    void Awake() {
        Invoke("SkipStartScreena", 0.25f);
        Invoke("SkipStartScreenb", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SkipStartScreena() {
        client.SendPacket(" ", HOLD_KEY);
    }
    void SkipStartScreenb() {
        client.SendPacket(" ", RELEASE_KEY);
    }
}
