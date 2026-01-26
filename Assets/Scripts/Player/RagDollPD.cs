using UnityEngine;

public class RagDollPD : MonoBehaviour
{
    public Rigidbody rb;
    public Transform target;

    [Header("PD Settings")]
    public float Kp = 500f;
    public float Kd = 50f;

    void FixedUpdate()
    {
        if(rb == null || target == null) return;

        Vector3 posError = target.position - rb.position; //目標位置との差
        Vector3 velError = -rb.linearVelocity;            //現在の速度との差(止まってほしい方向)
        Vector3 force = posError * Kp + velError * Kd;　　//PD制御の力
        rb.AddForce(force,ForceMode.Force);               //自然な力として適用

    }
}
