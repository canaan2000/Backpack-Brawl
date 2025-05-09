using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public Bounds collectionBounds;
    public List<GameObject> collectionObjs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnInCollection(GameObject objToCollection)
    {
        Vector3 randomPosition = new Vector3(
                Random.Range(collectionBounds.min.x, collectionBounds.max.x),
                Random.Range(collectionBounds.min.y, collectionBounds.max.y),
                Random.Range(collectionBounds.min.z, collectionBounds.max.z)
            );
        GameObject collectionObject = Instantiate(objToCollection, randomPosition, Quaternion.identity);
        collectionObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
