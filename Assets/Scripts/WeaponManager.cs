using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public float pickupRange;
    public float pickupRadius;

    public Transform weaponHolder;
    public Transform playerCamera;

    private bool _isWeaponHeld;
    private Weapon _heldWeapon;

    private void Update()
    {
        if(_isWeaponHeld)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _heldWeapon.Drop(playerCamera);
                _heldWeapon = null;
                _isWeaponHeld = false;
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange))
            {
                if (hit.transform.CompareTag("Weapon"))
                {
                    _isWeaponHeld = true;
                    _heldWeapon = hit.transform.GetComponent<Weapon>();
                    _heldWeapon.EquipWeapon(weaponHolder, playerCamera);
                }
            }
        }
    }
}
