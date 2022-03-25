using System.Collections;
using UnityEngine;

public class Viper : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject bomb;
    [Header("Transforms")]
    [SerializeField] Transform bombSpawner;
    [Header("Settings")]
    [SerializeField] float timeTillDropBomb = 5f;

    void OnEnable()
    {
        StartCoroutine(DropBomb());
    }

    IEnumerator DropBomb()
    {
        yield return new WaitForSeconds(timeTillDropBomb);
        Instantiate(bomb, bombSpawner.position, Quaternion.identity);
    }
}
