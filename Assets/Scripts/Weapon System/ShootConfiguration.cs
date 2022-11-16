using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Config", order = 2)]
public class ShootConfiguration : ScriptableObject
{
    public LayerMask HitMask;
    public Vector3 spread;
    public float fireRate;
}
