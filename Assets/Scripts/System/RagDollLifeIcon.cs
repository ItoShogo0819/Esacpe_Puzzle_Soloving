using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RagDollLifeIcon : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _rigidBodies;
    [SerializeField] private List<Collider> _colliders;
    [SerializeField] private Rigidbody _center;

    public void Exploed()
    {
        foreach (var rb in _rigidBodies) rb.isKinematic = false;
        foreach (var col in _colliders) col.enabled = true;

        Vector3 dir = (transform.up + transform.forward).normalized;
        _center.AddForce(dir * 250f, ForceMode.Impulse);

        foreach(var rb in _rigidBodies) rb.AddForce(Random.onUnitSphere * 80f, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }
}
