using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 200f;
    public bool isDead = false;

    public Collider hitbox;

    public float lookRadius;
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    public Ragdoll humanoid;

    public AudioClip[] takingDamageSounds;
    private List<AudioClip> potentialDamageSounds;
    private AudioClip lastSoundPlayed;

    public AudioSource source;

    private void Start()
    {
        hitbox.enabled = true;
    }

    private void Update()
    {
        if (isDead)
            return;
        float distance = Vector3.Distance(player.position, transform.position);

        if(distance <= lookRadius)
        {
            agent.SetDestination(player.position);

            if(distance <= agent.stoppingDistance)
            {
                FaceTarget();   
            }
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
            return;
        health -= amount;
        PlayRandomDamageSound();
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        hitbox.enabled = false;
        animator.enabled = false;
        agent.enabled = false;
        humanoid.ActivateRagdoll();
    }

    public void PlayRandomDamageSound()
    {
        potentialDamageSounds = new List<AudioClip>();

        foreach (var damageSound in takingDamageSounds)
        {
            if(damageSound != lastSoundPlayed)
            {
                potentialDamageSounds.Add(damageSound);
            }
        }

        int randomValue = Random.Range(0, potentialDamageSounds.Count);
        lastSoundPlayed = takingDamageSounds[randomValue];
        source.PlayOneShot(takingDamageSounds[randomValue]);
    }
}
