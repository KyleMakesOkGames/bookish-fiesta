using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Transform weaponContainer;
    public Transform playerCamera;
    private bool _isWeaponHeld;
    private Weapon heldWeapon;

    private void Update()
    {
        if(_isWeaponHeld)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                heldWeapon.Drop(playerCamera);
                heldWeapon = null;
                _isWeaponHeld = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4f))
            {
                if (hit.transform.CompareTag("Weapon"))
                {
                    _isWeaponHeld = true;
                    heldWeapon = hit.transform.GetComponent<Weapon>();
                    heldWeapon.PickUp(weaponContainer, playerCamera);
                }
            }
        }
    }
}
