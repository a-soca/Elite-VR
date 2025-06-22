using UnityEngine;

public class Critical_Info_Manager : MonoBehaviour
{
    // Alex Soca
    // Script for managing the Bar Controllers in the Critical Info Panel

    // Controllers for individual bars
    public Bar_Controller FrontShields;
    public Bar_Controller AftShields;

    public Shield_Controller ShieldGraphic;

    public Bar_Controller FuelVolume;
    public Bar_Controller Altitude;

    public Energy_Controller Energy;

    public Bar_Controller CabinTemperature;
    public Bar_Controller LaserTemperature;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the ENet Client and set this as the 
        GameObject.Find("ENetClient").GetComponent<ENet_Client>().criticalInfoManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePanel(string information) {
        string[] args = information.Split("|");

        // Health Stats
        FrontShields.UpdateValue(int.Parse(args[3]));
        AftShields.UpdateValue(int.Parse(args[4]));
        ShieldGraphic.UpdateValue(int.Parse(args[3]), int.Parse(args[4]));
        Energy.UpdateValue(int.Parse(args[5]));

        // Temperatures
        CabinTemperature.UpdateValue(int.Parse(args[6]));
        LaserTemperature.UpdateValue(int.Parse(args[7]));

        // Other
        Altitude.UpdateValue(int.Parse(args[8]));
        FuelVolume.UpdateValue(int.Parse(args[9])); // Important to note that this ranges from 0-70
    }
}
