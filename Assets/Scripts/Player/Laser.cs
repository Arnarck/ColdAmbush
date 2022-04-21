using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject collisionFX;
    [Header("Settings")]
    [SerializeField] float throwFactor = 10f;
    [SerializeField] float lifeTime = 1f;
    [SerializeField] float lifeTimeCollisionFX = .5f;

    Rigidbody _rigidBody;
    PoolingSystem _laserPool;
    PauseGame _pauseGame;
    Coroutine _deactivateLaser;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _laserPool = FindObjectOfType<PoolingSystem>();
        _pauseGame = FindObjectOfType<PauseGame>();
    }

    void OnEnable()
    {
        _deactivateLaser = StartCoroutine(DeactivateLaser(lifeTime));
    }

    IEnumerator DeactivateLaser(float time)
    {
        yield return new WaitForSeconds(time);
        SetCollisionEffects(false);
        SetLaserActive(true);
        _laserPool.AddToPool(gameObject);
    }

    void FixedUpdate()
    {
        if (!_pauseGame.IsPaused())
        {
            _rigidBody.velocity = transform.TransformDirection(Vector3.forward) * throwFactor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ProcessFatalCollision(other.tag);
    }

    void ProcessFatalCollision(string other)
    {
        if (other.Equals("Player") || other.Equals("Laser") || other.Equals("Finish")) { return; }

        StartCollisionSequence();
    }

    void StartCollisionSequence()
    {
        StopCoroutine(_deactivateLaser);
        SetLaserActive(false);
        SetCollisionEffects(true);
        StartCoroutine(DeactivateLaser(lifeTimeCollisionFX));
    }

    void SetLaserActive(bool isActive)
    {
        GetComponent<MeshRenderer>().enabled = isActive;
        GetComponent<SphereCollider>().enabled = isActive;
    }

    void SetCollisionEffects(bool isActive)
    {
        collisionFX.SetActive(isActive);
        GetComponent<Rigidbody>().isKinematic = isActive;
    }
}
