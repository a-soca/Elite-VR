using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading;

public class Start_BeebEm : MonoBehaviour
{
    // Alex Soca
    // Autostart script for BeebEm
    public ENet_Client client; // The Client used to communicate with the server

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Process p = new Process(); // Create a new process
        p.StartInfo.UseShellExecute = true; // Start the process using the system shell rather than as a child of the unity executable
        p.StartInfo.CreateNoWindow = false;
        
        p.StartInfo.FileName = Application.dataPath + @"\Resources\BeebEm.lnk"; // Specify the path of BeebEm

        p.Start(); // Start the process
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
