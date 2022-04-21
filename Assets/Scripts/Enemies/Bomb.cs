using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject detonationFX;
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject detonationRange;
    [SerializeField] GameObject detonationRangeVisual;
    [Header("Settings")]
    [SerializeField] float detonationTime = .3f;

    Transform _spawnAtRuntime;
    bool _isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        _spawnAtRuntime = GameObject.FindGameObjectWithTag("SpawnAtRuntime").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        ProcessFatalCollision(other.tag);
    }

    void ProcessFatalCollision(string other)
    {
        if (!_isAlive) { return; }// protects against two calls of this method in one frame

        if (other.Equals("Player"))
        {
            StartExplosionSequence();
        }
        else if (other.Equals("Laser") || other.Equals("Detonation"))
        {
            StartDetonationSequence();
        }
    }

    void StartExplosionSequence()
    {
        GameObject fx = Instantiate(explosionFX, transform.position, transform.rotation);
        fx.transform.SetParent(_spawnAtRuntime);
        _isAlive = false;
        Destroy(gameObject);
    }

    void StartDetonationSequence()
    {
        Transform playerTransform = GameObject.FindWithTag("Player").transform;
        GameObject fx = Instantiate(detonationFX, transform.position, playerTransform.rotation);

        _isAlive = false;
        fx.transform.SetParent(_spawnAtRuntime);
        detonationRange.SetActive(true);
        detonationRangeVisual.SetActive(false);

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        StartCoroutine(DestroyBomb());
    }

    IEnumerator DestroyBomb()
    {
        yield return new WaitForSeconds(detonationTime);
        Destroy(this.gameObject);
    }
}
