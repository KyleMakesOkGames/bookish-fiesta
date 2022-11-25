using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;
    public float force;

    public AIStateID GetID()
    {
        return AIStateID.Death;
    }

    public void Enter(AIAgent agent)
    {
        agent.ragdoll.EnableRagdoll();
        agent.ragdoll.ApplyForceToRagdoll(direction, force);
    }

    public void Update(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {

    }
}
