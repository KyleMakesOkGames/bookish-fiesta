using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum FireMode
{
    SemiAuto,
    FullAuto
}

public class WeaponBase : MonoBehaviour
{
    public bool fireLock = false;
    public bool isReloading = false;
    public bool shouldAim;
    public float damage;
    public float weaponImpactForce;
    public float range;
    public float fireRate;
    public int amountOfBulletsToShoot;
    public AudioSource source;

    public AudioClip shotSFX;
    public AudioClip pickedUp;

    public FireMode fireMode = FireMode.FullAuto;

    public TMP_Text text;

    public Vector3 normalPosition;
    public Vector3 normalAimingPosition;
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
    Vector3 desiredPosition;

    public float reloadTime;
    public int maxAmmo;
    public int clipSize;
    public int bulletsInClip;
    public int bulletsLeft;

    public Vector2 spread;

    private void Start()
    {
        bulletsInClip = clipSize;
        bulletsLeft = maxAmmo;
    }

    private void Update()
    {
        if (fireMode == FireMode.FullAuto && Input.GetButton("Fire1"))
        {
            CheckFire();
        }
        else if (fireMode == FireMode.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            CheckFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckReload();
        }

        if (Input.GetMouseButton(1))
        {
            desiredPosition = normalAimingPosition;
        }
        else
        {
            desiredPosition = normalPosition;
        }

        text.SetText(bulletsInClip + " / " + bulletsLeft);
    }

    private void FixedUpdate()
    {
        rotationRecoil = Vector3.Lerp(rotationRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, desiredPosition, positionalReturnSpeed * Time.deltaTime);

        transform.localPosition = Vector3.Slerp(transform.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        rot = Vector3.Slerp(rot, rotationRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(rot);
    }

    private void CheckFire()
    {
        if (fireLock)
            return;
        if (isReloading)
            return;
        if (bulletsInClip > 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        fireLock = true;

        for (int i = 0; i < amountOfBulletsToShoot; i++)
        {
            DetectHit();
        }

        ApplyRecoil();
        source.PlayOneShot(shotSFX);
        bulletsInClip--;
        StartCoroutine(CoResetFireLock());
    }

    private void DetectHit()
    {
        RaycastHit hit;

        float x = Random.Range(-spread.x, spread.x);
        float y = Random.Range(-spread.y, spread.y);

        Vector3 direction = Camera.main.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(Camera.main.transform.position, direction, out hit))
        {
            Debug.Log(hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.TakeDamage(damage);

                if (enemy.isDead)
                {
                    enemy.GetComponentInChildren<Ragdoll>().ApplyForceToRagdoll(Camera.main.transform.forward, weaponImpactForce);
                }
            }
        }
    }

    IEnumerator CoResetFireLock()
    {
        yield return new WaitForSeconds(fireRate);
        fireLock = false;
    }

    public void ApplyRecoil()
    {
        if (Input.GetMouseButton(1))
        {
            rotationRecoil += new Vector3(-recoilRotationAim.x, Random.Range(-recoilRotationAim.y, recoilRotationAim.y), Random.Range(-recoilRotationAim.z, recoilRotationAim.z));
            positionalRecoil += new Vector3(Random.Range(-recoilKickBackAim.x, recoilKickBackAim.x), Random.Range(-recoilKickBackAim.y, recoilKickBackAim.y), recoilKickBackAim.z);
        }
        else
        {
            rotationRecoil += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
            positionalRecoil += new Vector3(Random.Range(-recoilKickBack.x, recoilKickBack.x), Random.Range(-recoilKickBack.y, recoilKickBack.y), recoilKickBack.z);
        }
    }

    private void CheckReload()
    {
        if (bulletsLeft > 0 && bulletsInClip < clipSize)
        {
            Reload();
        }
    }

    private void Reload()
    {
        if (isReloading)
            return;
        isReloading = true;
        StartCoroutine(ReloadAmmo());
    }

    IEnumerator ReloadAmmo()
    {
        yield return new WaitForSeconds(reloadTime);
        int bulletsToLoad = clipSize - bulletsInClip;
        int bulletsToSub = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToSub;
        bulletsInClip += bulletsToLoad;
        isReloading = false;
    }
}
