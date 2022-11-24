using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private Transform RagdollRoot;
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private bool StartRagdoll = false;
    private Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;

    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();

        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
    }

    private void Start()
    {
        if (StartRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }

    public void EnableRagdoll()
    {
        Animator.enabled = false;
        Agent.enabled = false;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
    }

    public void DisableAllRigidbodies()
    {
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }

    public void EnableAnimator()
    {
        Animator.enabled = true;
        Agent.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            //rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }

    public void ApplyForceToRagdoll(Vector3 hitPoint, float amountOfForce)
    {
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.AddForce(amountOfForce * hitPoint.normalized, ForceMode.Impulse);
        }
    }
}
