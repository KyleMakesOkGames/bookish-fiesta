using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 cameraOffset; // Offset of the camera from the player.

    private void Update()
    {
        CameraFollow();
    }

    private void CameraFollow()
    {
        Vector3 targetCamPos = cameraTransform.position + cameraOffset;
        transform.position = targetCamPos;
    }
}
