using UnityEngine;
using System.Collections.Generic;

public class InputBuffer : MonoBehaviour
{
    public static InputBuffer Instance;

    public class Record
    {
        public Vector2 Dir;
        public float Time;
        public Record(Vector2 d, float t)
        {
            Dir = d; Time = t;
        }
    }

    public List<Record> buffer = new();
    public float bufferTime = 0.5f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        buffer.Add(new Record(dir,Time.time));

        buffer.RemoveAll(r => Time.time -  r.Time > bufferTime);
    }
}
