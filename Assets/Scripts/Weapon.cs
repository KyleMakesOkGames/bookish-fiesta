using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
    SemiAuto,
    FullAuto
}

public class Weapon : MonoBehaviour
{
    public bool currentWeapon;
    public bool fireLock = false;
    public bool isEquipped;
    public float damage;
    public float spread;
    public float range;
    public float fireRate;
    public int amountOfBulletsToShoot;
    public AudioSource source;

    public AudioClip shotSFX;
    public AudioClip pickedUp;

    public FireMode fireMode = FireMode.FullAuto;

    private Rigidbody _rb;

    public Collider[] gfxColliders;

    private Transform _playerCamera;

    [Header("RECOIL")]
    public Vector3 normalPosition;
    public float positionalRecoilSpeed;
    public float rotationalRecoilSpeed;

    public float positionalReturnSpeed;
    public float rotationalReturnSpeed;

    public Vector3 recoilRotation = new Vector3(10, 5, 7);
    public Vector3 recoilKickBack = new Vector3(0.015f, 0f, -0.2f);

    public Vector3 recoilRotationAim = new Vector3(10, 4, 6);
    public Vector3 recoilKickBackAim = new Vector3(0.015f, 0f, -0.2f);

    Vector3 rotationRecoil;
    Vector3 positionalRecoil;
    Vector3 rot;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isEquipped)
            return;
        if (fireMode == FireMode.FullAuto && Input.GetButton("Fire1"))
        {
            CheckFire();
        }
        else if (fireMode == FireMode.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            CheckFire();
        }
    }

    private void FixedUpdate()
    {
        if (!isEquipped)
            return;
        rotationRecoil = Vector3.Lerp(rotationRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, normalPosition, positionalReturnSpeed * Time.deltaTime);

        transform.localPosition = Vector3.Slerp(transform.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        rot = Vector3.Slerp(rot, rotationRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(rot);
    }

    private void CheckFire()
    {
        if (fireLock)
            return;
        Fire();
    }

    private void Fire()
    {
        fireLock = true; 
        source.PlayOneShot(shotSFX);
        ApplyRecoil();
        for (int i = 0; i < amountOfBulletsToShoot; i++)
        {
            DetectHit();
        }
        StartCoroutine(CoResetFireLock());
    }

    private void DetectHit()
    {
        RaycastHit hit;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = _playerCamera.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(_playerCamera.transform.position, direction, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Target target = hit.transform.GetComponent<Target>();
                target.TakeDamage(damage);
                //target.CreateBlood(hit.point, hit.normal);
            }
        }
    }

    public void PickUp(Transform weaponHolder, Transform playerCamera)
    {
        if (isEquipped)
            return;
        _rb.isKinematic = true;

        transform.parent = weaponHolder;
        source.PlayOneShot(pickedUp);
        foreach (var col in gfxColliders)
        {
            col.isTrigger = true;
        }

        isEquipped = true;

        _playerCamera = playerCamera;
    }

    public void Drop(Transform playerCamera)
    {
        if (!isEquipped)
            return;
        _rb.isKinematic = false;
        var forward = playerCamera.forward;
        _rb.AddForce(forward * 10, ForceMode.Impulse);
        
        foreach (var col in gfxColliders)
        {
            col.isTrigger = false;
        }
        transform.parent = null;
        isEquipped = false;
    }

    IEnumerator CoResetFireLock()
    {
        yield return new WaitForSeconds(fireRate);
        fireLock = false;
    }

    public void ApplyRecoil()
    {
        rotationRecoil += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
        positionalRecoil += new Vector3(Random.Range(-recoilKickBack.x, recoilKickBack.x), Random.Range(-recoilKickBack.y, recoilKickBack.y), recoilKickBack.z);
    }
}
