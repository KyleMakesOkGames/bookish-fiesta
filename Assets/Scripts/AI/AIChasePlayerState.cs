using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIChasePlayerState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
            return;

        agent.navMeshAgent.destination = agent.playerTransform.position;
        agent.animator.SetFloat("Speed", agent.navMeshAgent.velocity.magnitude);

    }

    public void Exit(AIAgent agent)
    {
        
    }
}
