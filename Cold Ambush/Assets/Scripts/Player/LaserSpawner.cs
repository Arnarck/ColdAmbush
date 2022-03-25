using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject laser;
    [Header("Audio Clips")]
    [SerializeField] AudioClip laserSFX;
    [Header("Settings")]
    [SerializeField] float coolDown = .05f;
    
    PoolingSystem _laserPool;
    Transform _spawnAtRuntime;
    AudioSource _audioSource;

    bool canShoot;

    void Start()
    {
        _spawnAtRuntime = GameObject.FindWithTag("SpawnAtRuntime").transform;
        _audioSource = GameObject.FindWithTag("GameSystem").GetComponent<AudioSource>();
        _laserPool = FindObjectOfType<PoolingSystem>();
    }

    void OnEnable()
    {
        canShoot = true;
    }

    public void Shoot()
    {
        if (!canShoot) { return; }

        GameObject laserInstance = _laserPool.InsertInTheWorld();
        AdjustProjectileCoordinates(laserInstance);
        _audioSource.PlayOneShot(laserSFX);
        canShoot = false;
        StartCoroutine(CoolDown());
    }

    void AdjustProjectileCoordinates(GameObject projectile)
    {
        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;
        projectile.transform.SetParent(_spawnAtRuntime);
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDown);
        canShoot = true;
    }
}
