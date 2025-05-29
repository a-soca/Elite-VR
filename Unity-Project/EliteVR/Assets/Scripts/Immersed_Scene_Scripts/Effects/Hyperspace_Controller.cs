using UnityEngine;

public class Hyperspace_Controller : MonoBehaviour
{
    // Alex Soca
    // Script to start and stop the hyperspace transition effect
    public ParticleSystem hyperspace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterHyperspace() {
        hyperspace.enableEmission = true;
    }

    public void ExitHyperspace() {
        hyperspace.enableEmission = false;
    }
}
