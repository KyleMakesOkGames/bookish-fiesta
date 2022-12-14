using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIChasePlayerState : AIState
{
    
    float timer;
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

        timer -= Time.deltaTime;
        if(!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if(timer < 0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if(direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }

    }

    public void Exit(AIAgent agent)
    {
        
    }

    

    
}
