using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject objectPrefab;

    Queue<GameObject> _objectsPool = new Queue<GameObject>();

    public GameObject InsertInTheWorld()
    {
        GameObject generatedObject;
        if (_objectsPool.Count > 0)
        {
            generatedObject = _objectsPool.Dequeue();
            generatedObject.SetActive(true);
        }
        else
        {
            generatedObject = Instantiate(objectPrefab);
        }

        return generatedObject;
    }

    public void AddToPool(GameObject deactivatedObject)
    {
        deactivatedObject.SetActive(false);
        _objectsPool.Enqueue(deactivatedObject);
    }
}
