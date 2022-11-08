using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;

    private Rigidbody rb;
    private Collider itemcollider;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        itemcollider = GetComponent<Collider>();
        itemcollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;
        Debug.Log(collision.transform.name);

        // check if you hit an enemy
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(damage);

            // destroy projectile
            Destroy(gameObject);
        }

        // make sure projectile sticks to surface
        rb.isKinematic = true;
        itemcollider.enabled = false;

        // make sure projectile moves with target
        transform.SetParent(collision.transform);
    }
}