using UnityEngine;


public enum FlagColor
{
    Red,White
}

public enum ArmOrder
{
    None,Up,Down
}

public class ArmData : MonoBehaviour
{
    [SerializeField] public Transform LeftArm;
    [SerializeField] public Transform RightArm;
    [SerializeField] public Transform Chest;
    //public FlagColor Color;
    public bool HasMoved;
}
