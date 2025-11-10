using UnityEngine;

public class GrabChecker : MonoBehaviour
{
    public ArmController armController;
    public Transform LeftHand;
    public Transform RightHand;
    public float GrabRange = 0.3f;

    private GameObject _leftGrabbed;
    private GameObject _rightGrabbed;

    void Update()
    {
        if (armController._leftGrip)
        {
            if (_leftGrabbed == null)
            {
                //TryGrab(LeftHand, ref _leftGrabbed);
            }
                
        }
    }

    void TryGrab()
    {

    }
}
