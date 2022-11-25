using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{
    public Transform playerTransform;

    NavMeshAgent agent;
    Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        agent.SetDestination(playerTransform.position);
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
