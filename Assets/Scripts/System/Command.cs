using UnityEngine;

[System.Serializable]
public class Command : MonoBehaviour
{
    public string Name;
    public Vector2[] Sequence;
    public float Lenience = 0.45f;

    public Command(string name, Vector2[] seq)
    {
        this.Name = name;
        this.Sequence = seq;
    }
}
