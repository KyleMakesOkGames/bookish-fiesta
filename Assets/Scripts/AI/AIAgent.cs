using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public Transform playerTransform;
    public AIStateMachine stateMachine;
    public AIStateID initialState;
    public NavMeshAgent navMeshAgent;
    public Ragdoll ragdoll;
    public AIAgentConfiguration config;

    private void Awake()
    {
        stateMachine = new AIStateMachine(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<Ragdoll>();
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.ChangeState(initialState);

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
