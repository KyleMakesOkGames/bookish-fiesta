using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovementFinished pm;

    public float dashForwardForce;
    public float dashUpwardForce;
    public float dashDuration;


}
