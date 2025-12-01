using UnityEngine;
using UnityEngine.InputSystem;
public class ArmController : MonoBehaviour
{
    public Rigidbody LeftArm, RightArm;
    public Vector2 LeftMove, RightMove;
    public bool _leftGrip, _rightGrip;
    public float ForcePower = 50.0f;

    void Start()
    {
        Debug.Log("起動しました");
    }

    public void LeftArmMove(InputAction.CallbackContext cont)
    {
        LeftMove = cont.ReadValue<Vector2>();
        //Debug.Log("左腕を確認");
    }

    public void RightArmMove(InputAction.CallbackContext cont)
    {
        RightMove = cont.ReadValue<Vector2>();
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
        Vector3 leftForce = new Vector3(LeftMove.x, LeftMove.y, 0) * ForcePower;
        Vector3 rightForce = new Vector3(RightMove.x, RightMove.y, 0) * ForcePower;

        LeftArm.AddForce(leftForce, ForceMode.Acceleration);
        RightArm.AddForce(rightForce, ForceMode.Acceleration);
        
        //LeftArm.position += new Vector3(LeftMove.x, LeftMove.y, 0) * MoveSpeed * Time.deltaTime;
        //RightArm.position += new Vector3(RightMove.x, RightMove.y, 0) * MoveSpeed * Time.deltaTime;
    }
}
