using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Knockback : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] [Range(0, 100)] float magnitude;
    [SerializeField] ForceMode forceMode;
    [SerializeField] bool away;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody) && !away)
        {
            rigidbody.AddForce(direction * magnitude, forceMode);
        }
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody2) && away)
        {
            Vector3 direction2 = -(this.transform.position - other.transform.position).normalized;
            rigidbody2.AddForce(direction2 * magnitude, forceMode);
        }
    }
}
