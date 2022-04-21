using System.Collections;
using UnityEngine;

public class Interceptor : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject missile;
    [SerializeField] GameObject[] missileSpawners;
    [Header("Settings")]
    [SerializeField] float timeTillLaunchMissile;


    void OnEnable()
    {
        StartCoroutine(LaunchMissile());
    }

    IEnumerator LaunchMissile()
    {
        yield return new WaitForSeconds(timeTillLaunchMissile);

        for(int i = 0; i < missileSpawners.Length; i++)
        {
            Instantiate(missile, missileSpawners[i].transform.position, transform.rotation);
        }
    }
}
