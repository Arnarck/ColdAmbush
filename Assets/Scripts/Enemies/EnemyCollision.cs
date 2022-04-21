using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject engineFailures;
    [Header("Settings")]
    [SerializeField] int maxHitPoints = 2;
    [SerializeField] int scoreByKill;

    Transform _spawnAtRuntime;
    DamageList _damageList;

    int _currentHitPoints;

    void Start()
    {
        _currentHitPoints = maxHitPoints;
        _damageList = FindObjectOfType<DamageList>();
        _spawnAtRuntime = GameObject.FindGameObjectWithTag("SpawnAtRuntime").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        ProcessFatalCollision(other.tag);
    }

    void ProcessFatalCollision(string other)
    {
        if (_currentHitPoints < 1) { return; } // protects agains two calls of this method in one single frame

        if (other.Equals("Player"))
        {
            KillEnemy();
        }
        else if (other.Equals("Laser") || other.Equals("Detonation"))
        {
            StartDamageTakenSequence(other);
        }
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
            IncreasePlayerStats(); // only update player's stats when the enemy is killed by the laser or detonation
            KillEnemy();
        }
    }

    void ProcessHit(int hit)
    {
        _currentHitPoints -= hit;
        // Activate some effects that shows that the enemy is hurt when he's with 50% or less of life
        if (_currentHitPoints <= maxHitPoints / 2)
        {
            engineFailures.SetActive(true);
        }
    }

    void IncreasePlayerStats()
    {
        FindObjectOfType<Scoreboard>().IncreaseScore(scoreByKill);
        FindObjectOfType<BulletHellAttack>().Charge(scoreByKill);
    }

    void KillEnemy()
    {
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        fx.transform.SetParent(_spawnAtRuntime);
        _currentHitPoints = 0;
        Destroy(gameObject);
    }
}
