using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    Vector2 moveInput;

    public float MoveSpeed = 5f;
    public bool IsControllable = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized * MoveSpeed; 
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);                      
    }

    public void ResetInput()
    {
        moveInput = Vector3.zero;
    }
}
