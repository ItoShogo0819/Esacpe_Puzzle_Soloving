using UnityEngine;
using UnityEngine.InputSystem;
public class ArmController : MonoBehaviour
{
    [SerializeField] public Rigidbody LeftArm;
    [SerializeField] public Rigidbody RightArm;

    [SerializeField] private float _forcePower = 50.0f;


    private Vector3 _leftMove;
    private Vector3 _rightMove;
    public bool _leftGrip;
    public bool _rightGrip;

    void Start()
    {
        Debug.Log("起動しました");
    }

    public void LeftArmMove(InputAction.CallbackContext cont)
    {
        _leftMove = cont.ReadValue<Vector2>();
        //Deug.Log("左腕を確認");
    }

    public void RightArmMove(InputAction.CallbackContext cont)
    {
        _rightMove = cont.ReadValue<Vector2>();
        //Debug.Log("右腕を確認");
    }

    public void LeftGrip(InputAction.CallbackContext cont)
    {
        _leftGrip = cont.ReadValueAsButton();
        //Debug.Log("左手を確認");
    }

    public void RightGrip(InputAction.CallbackContext cont)
    {
        _rightGrip = cont.ReadValueAsButton();
        //Debug.Log("右手を確認");
    }

    void FixedUpdate()
    {
        var leftForce = new Vector3(_leftMove.x, _leftMove.y, 0f) * _forcePower;
        var rightForce = new Vector3(_rightMove.x, _rightMove.y, 0f) * _forcePower;

        LeftArm.AddForce(leftForce, ForceMode.Acceleration);
        RightArm.AddForce(rightForce, ForceMode.Acceleration);
    }
}
