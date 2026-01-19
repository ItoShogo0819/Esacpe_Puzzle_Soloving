using UnityEngine;


public enum FlagColor
{
    Red,White
}

public enum ArmOrder
{
    None,Up,Down
}

[System.Serializable]
public class ArmData : MonoBehaviour
{
    public Transform Arm;
    public FlagColor Color;
    public bool HasMoved;
}
