using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] ragdollBodies;
    public Transform test;
    public Collider[] ragdollColliders;

    private void Awake()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = false;
        }
    }

    public void ActivateRagdoll()
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
        }

        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }
    }

    public void ApplyForceToRagdoll(Vector3 hitPoint, float amountOfForce)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.AddForce(amountOfForce * hitPoint.normalized, ForceMode.Impulse);
        }
    }
}
