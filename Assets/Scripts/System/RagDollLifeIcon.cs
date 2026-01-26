using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagDollLifeIcon : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _rigidBodies;
    [SerializeField] private List<Collider> _colliders;
    [SerializeField] private Rigidbody _center;

    void Awake()
    {
        _rigidBodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        _colliders = new List<Collider>(GetComponentsInChildren<Collider>());

        SetRagdoll(false);
    }

    public void Explode()
    {
        StartCoroutine(ExplodeRoutine());
    }

    IEnumerator ExplodeRoutine()
    {
        SetRagdoll(true);
        yield return new WaitForFixedUpdate();

        Vector3 dir = (transform.up + transform.forward).normalized;
        _center.AddForce(dir * 120f, ForceMode.Impulse);

        foreach (var rb in _rigidBodies)
            rb.AddForce(Random.onUnitSphere * 30f, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }

    void SetRagdoll(bool enable)
    {
        foreach (var rb in _rigidBodies)
            rb.isKinematic = !enable;

        foreach (var col in _colliders)
            col.enabled = enable;
    }
}