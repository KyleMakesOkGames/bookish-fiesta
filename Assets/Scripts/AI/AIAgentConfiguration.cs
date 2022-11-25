using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class AIAgentConfiguration : ScriptableObject
{
    public float maxTime;
    public float maxDistance;
    public float maxSightDistance;
}
