using UnityEngine;


public enum ArmOrder
{
    None,Up,Down
}

public struct InstructionSet
{
    public ArmOrder Left;
    public ArmOrder Right;
}

public class ArmData : MonoBehaviour
{
    [SerializeField] public Transform LeftArm;
    [SerializeField] public Transform RightArm;
    [SerializeField] public Transform Chest;
    public bool HasMoved;
}
