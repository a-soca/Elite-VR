using UnityEngine;

public class Flash_Object : MonoBehaviour
{
    // Alex Soca
    // A script to make the object it is attached to flash at a given interval and offset
    public float rate = 0.1f;
    public float offset = 0f;
    Renderer renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = this.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.enabled = ((offset+Time.fixedTime) % rate < rate/2f);
    }
}
