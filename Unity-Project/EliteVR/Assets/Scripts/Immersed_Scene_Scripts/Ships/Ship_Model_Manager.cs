using UnityEngine;
using System.Collections.Generic;
public class Ship_Model_Manager : MonoBehaviour
{
    // Alex Soca
    // Script to manage loading and providing models to Model Updater scripts
    string[] shipNames = {
        // Name         ID
        "missile",      // 01
        "coriolis",     // 02
        "escape-pod",   // 03
        "plate",        // 04
        "canister",     // 05
        "boulder",      // 06
        "asteroid",     // 07
        "splinter",     // 08
        "shuttle",      // 09
        "transporter",  // 10
        "cobra_mk_3",   // 11
        "python",       // 12
        "boa",          // 13
        "anaconda",     // 14
        "rock_hermit",  // 15
        "viper",        // 16
        "sidewinder",   // 17
        "mamba",        // 18
        "krait",        // 19
        "adder",        // 20
        "gecko",        // 21
        "cobra_mk_1",   // 22
        "worm",         // 23
        "cobra_mk_3_p", // 24
        "asp_mk_2",     // 25
        "python_p",     // 26
        "fer_de_lance", // 27
        "moray",        // 28
        "thargoid",     // 29
        "thargon",      // 30
        "constrictor",  // 31
        "elite_logo",   // 32
        "cougar",       // 33
        "dodo",         // 34
        "planet_e",      // 128
        "planet_c"      // 130
        // Need to add this model, for now they will be replaced with the planet
        // "sun",          // 129
    };

    private List<Mesh[]> meshes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshes = new List<Mesh[]>(); // Initialise the list of meshes

        // Load all of the meshes up front to prevent slowdowns at runtime
        for(int i = 0; i < shipNames.Length; i++) {
            meshes.Add(Resources.LoadAll<Mesh>("EliteModels/" + shipNames[i])); // Load the model from the resources folder
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Mesh[] GetMeshesByShipID(int ID) {
        if(ID > 34 || ID < 1) {
            if(ID == 128) {
                return meshes[34]; // planet_e
            } else if(ID == 130) {
                return meshes[35]; // planet_c
            } else {
                return meshes[34]; // planet_e TEMPORARY
            }
        } else {
            return meshes[ID-1];
        }
    }
}
