using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public GunType type;
    public string wepName;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;

    public Transform camera;

    public ShootConfiguration shootConfig;

    private MonoBehaviour currentMonobehaviour;
    private GameObject model;
    private float lastShootTime;

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this.currentMonobehaviour = activeMonoBehaviour;
        lastShootTime = 0;
        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);
    }

    public void Shoot()
    {
        if(Time.time > shootConfig.fireRate + lastShootTime)
        {
            Vector3 shootDirection = camera.transform.forward + new Vector3(
                    Random.Range(-shootConfig.spread.x, shootConfig.spread.x),
                    Random.Range(-shootConfig.spread.y, shootConfig.spread.y),
                    Random.Range(-shootConfig.spread.z, shootConfig.spread.z));
            shootDirection.Normalize();       

            if(Physics.Raycast(camera.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.HitMask))
            {

            }
        }
    }
}
