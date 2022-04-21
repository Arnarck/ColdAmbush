using System.Collections;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject detonationFX;
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject detonationRange;
    [Header("Settings")]
    [SerializeField] float thrustFactor = 20f;
    [SerializeField] float lifeTime = 5f;

    Rigidbody _rigidBody;
    AudioSource _audioSource;
    Transform _spawnAtRuntime;
    PauseGame _pauseGame;

    bool _isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidBody = GetComponent<Rigidbody>();
        _pauseGame = FindObjectOfType<PauseGame>();
        _spawnAtRuntime = GameObject.FindWithTag("SpawnAtRuntime").transform;
        StartCoroutine(DestroyDelayed());
    }

    void Update()
    {
        SetMissileThrusterVolume();
    }

    void SetMissileThrusterVolume()
    {
        if (_pauseGame.IsPaused())
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
        else
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
    }

    void FixedUpdate()
    {
        ProcessMovement();
    }

    void ProcessMovement()
    {
        if (_isAlive)
        {
            _rigidBody.velocity = transform.TransformDirection(Vector3.forward) * thrustFactor;
        }
        else
        {
            _rigidBody.velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ProcessFatalCollision(other.tag);
    }

    void ProcessFatalCollision(string other)
    {
        if (!_isAlive) { return; }

        if (other.Equals("Player") || other.Equals("Laser") || other.Equals("Detonation"))// explosion
        {
            StartExplosionSequence();
        }
    }

    void StartExplosionSequence()
    {
        GameObject fx = Instantiate(explosionFX, transform.position, Quaternion.identity);
        fx.transform.SetParent(_spawnAtRuntime);
        _isAlive = false;
        Destroy(gameObject);
    }

    IEnumerator DestroyDelayed()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
