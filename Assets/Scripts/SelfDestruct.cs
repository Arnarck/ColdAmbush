using System.Collections;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float timeTillDestroy = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyDelayed());
    }

    IEnumerator DestroyDelayed()
    {
        yield return new WaitForSeconds(timeTillDestroy);
        Destroy(gameObject);
    }
}
