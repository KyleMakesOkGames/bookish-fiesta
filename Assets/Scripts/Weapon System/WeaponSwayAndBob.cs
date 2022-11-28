using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwayAndBob : MonoBehaviour
{
    public Rigidbody rb;
    public PlayerMovementFinished pm;

    public bool sway = true;
    public bool swayRotation = true;
    public bool bobOffset;
    public bool bobSway;

    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    public float rotationStep = 4f;
    public float maxRotation = 5f;
    Vector3 swayEulerRot;

    Vector2 walkInput;
    Vector2 mouseInput;

    public float speedCurve;

    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;

    Vector3 bobPosition;

    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    private void Update()
    {
        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        //Apply All of our movment and rotation components
        CompositePositionRotation();
    }
    
    private void GetInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        mouseInput = new Vector2(mouseX, mouseY);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        walkInput = new Vector2(moveX, moveY);
    }

    private void Sway()
    {
        if(sway == false) { swayPos = Vector3.zero; return; }

        Vector3 invertLook = mouseInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    private void SwayRotation()
    {
        if (swayRotation == false) { swayEulerRot = Vector3.zero; return; }

        Vector2 invertLook = mouseInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotation, maxRotation);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotation, maxRotation);

        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    private void BobOffset()
    {
        speedCurve += Time.deltaTime * (pm.grounded ? rb.velocity.magnitude : 1f) + 0.01f;

        if (bobOffset == false) { bobPosition = Vector3.zero; return; }

        bobPosition.x = (curveCos * bobLimit.x * (pm.grounded ? 1 : 0)) - (walkInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (rb.velocity.y * travelLimit.y);
        bobPosition.z = - (walkInput.y * travelLimit.z);
    }

    private void BobRotation()
    {
        if (bobSway == false) { bobEulerRotation = Vector3.zero; return; }

        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));

        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
    }

    float smooth = 10f;
    float smoothRot = 12f;

    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }
}
