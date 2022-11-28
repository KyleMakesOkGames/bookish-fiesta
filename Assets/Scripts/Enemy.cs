using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;
    public AIAgent agent;
    public float health = 200f;
    public bool isDead = false;

    public void TakeDamage(float amount, float force, Vector3 direction)
    {
        if (isDead)
            return;
        health -= amount;
        if (health <= 0f)
        {
            Die(direction, force);
            OnEnemyKilled?.Invoke(this);
        }
    }

    private void Die(Vector3 direction, float force)
    {
        AIDeathState deathState = agent.stateMachine.GetState(AIStateID.Death) as AIDeathState;
        deathState.direction = direction;
        deathState.force = force;
        agent.stateMachine.ChangeState(AIStateID.Death);
    }
}
