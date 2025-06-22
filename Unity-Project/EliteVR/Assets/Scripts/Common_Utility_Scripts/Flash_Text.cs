using UnityEngine;

public class Flash_Text : MonoBehaviour
{
    public float rate = 0.1f;
    public float offset = 0f;
    GameObject text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        text.SetActive(((offset+Time.fixedTime) % rate < rate/2f));
    }
}
