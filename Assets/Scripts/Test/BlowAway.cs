using UnityEngine;

public class BlowAway : MonoBehaviour
{
    public float Pba = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 a = new Vector3(0, 0, 2 * Pba);
        }
    }
}
