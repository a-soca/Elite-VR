using UnityEngine;
using System.Collections.Generic;

public class Ship_Model_Updater : MonoBehaviour
{
    // Alex Soca
    // Updates the mesh of the ship in accordance with the provided ship ID
    private Ship_Model_Manager modelManager; // Script which provides model meshes based on ship ID
    private MeshFilter modelMeshFilter; // The component which houses the opaque part of the model
    private MeshFilter wireframeMeshFilter; // The component which houses the wireframe of the model

    // Variables to store the ID of the ship
    private int currentID = 0;
    private int ID = 0;

    public bool sunSlot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the model manager
        modelManager = this.transform.GetComponentInParent<Ship_Model_Manager>();

        // Find the mesh filter components
        modelMeshFilter = this.transform.Find("Model").transform.GetComponent<MeshFilter>();
        wireframeMeshFilter = this.transform.Find("Wireframe").transform.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentID != ID) { // If the ID has changed,
            currentID = ID; // Update the current ID
            if(ID != 129) {
                if(sunSlot)
                    this.gameObject.transform.Find("Sun").gameObject.SetActive(false);

                Mesh[] meshes = modelManager.GetMeshesByShipID(ID); // Get the new meshes from the manager
                modelMeshFilter.mesh = meshes[0]; // Set the model
                wireframeMeshFilter.mesh = meshes[1]; // Set the wireframe
            } else if(sunSlot) {
                this.gameObject.transform.Find("Sun").gameObject.SetActive(true);
            }
        }
    }

    public void SetModelByID(int providedID) {
        ID = providedID; // Update the ID variable
    }

    public int GetID() {
        return ID;
    }
}
