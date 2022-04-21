using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisions : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject engineFailures;
    [SerializeField] GameObject playerGameJuice;
    [Header("Sliders")]
    [SerializeField] Slider lifeBar;
    [Header("Audio Clips")]
    [SerializeField] AudioClip hitSFX;
    [Header("Settings")]
    [SerializeField] int maxHitPoints = 100;

    DamageList _damageList;
    Transform _spawnAtRuntime;
    AudioSource _audioSource;

    int _currentHitPoints;

    void Start()
    {
        _damageList = FindObjectOfType<DamageList>();
        _spawnAtRuntime = GameObject.FindWithTag("SpawnAtRuntime").transform;
        _audioSource = GameObject.FindWithTag("GameSystem").GetComponent<AudioSource>();

        _currentHitPoints = maxHitPoints;
        lifeBar.maxValue = maxHitPoints;
        lifeBar.value = _currentHitPoints;
    }

    void OnTriggerEnter(Collider other)
    {
        ProcessFatalCollision(other.tag);
    }

    void ProcessFatalCollision(string other)
    {
        if (other.Equals("Laser") || other.Equals("Untagged") || _currentHitPoints < 1) { return; }

        StartDamageTakenSequence(other);
    }

    void StartDamageTakenSequence(string other)
    {
        int hit = _damageList.GetDamageFromCollisionWith(other);
        if (hit > 0)
        {
            ProcessHit(hit);
        }

        if (_currentHitPoints < 1)
        {
            StartDeathSequence();
        }
    }

    void ProcessHit(int hit)
    {
        _currentHitPoints -= hit;
        _audioSource.PlayOneShot(hitSFX);
        if (_currentHitPoints < 0)
        {
            _currentHitPoints = 0;
        }

        lifeBar.value = _currentHitPoints;
        if (_currentHitPoints <= maxHitPoints / 2) // activate the "hurt" fx when the player is with half or less health
        {
            engineFailures.SetActive(true);
        }
    }

    void StartDeathSequence()
    {
        GameObject fx = Instantiate(explosionFX, transform.position, Quaternion.identity);
        fx.transform.SetParent(_spawnAtRuntime);

        GetComponent<PlayerControls>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        playerGameJuice.SetActive(false);

        _currentHitPoints = 0;
        Time.timeScale = .25f;
        StartCoroutine(DelayGameOver());
    }

    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<PauseGame>().ActivateGameOver();
    }

    public bool IsAlive()
    {
        return _currentHitPoints > 1;
    }
}
