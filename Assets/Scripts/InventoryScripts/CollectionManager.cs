using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public GameObject collectionBounds;
    public List<GameObject> collectionObjs;

    Vector3 maxSpawn;
    Vector3 minSpawn;
    Vector3 spawn;
    // Start is called before the first frame update
    void Start()
    {
        minSpawn = collectionBounds.GetComponent<Collider>().bounds.min;
        maxSpawn = collectionBounds.GetComponent<Collider>().bounds.max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnInCollection(GameObject objToCollection)
    {
        spawn = new Vector3(
                Random.Range(minSpawn.x, maxSpawn.x),
                Random.Range(minSpawn.y, maxSpawn.y),
                Random.Range(minSpawn.z, maxSpawn.z)
            );
        GameObject collectionObject = Instantiate(objToCollection, spawn, Quaternion.identity);
        collectionObjs.Add(collectionObject);
        collectionObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
