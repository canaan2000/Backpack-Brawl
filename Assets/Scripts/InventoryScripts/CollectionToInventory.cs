using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionToInventory : MonoBehaviour
{
    public CollectionManager Collection;
    public SpawnerScript Spawner;
    public InventoryList InventoryList;

    public int maxInventory = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InventoryList.inventoryList.Count < maxInventory)
        {
            SpawnFromCollection();
        }
    }

    public void SpawnFromCollection()
    {
        int i = Random.Range(0, Collection.collectionObjs.Count);
        Spawner.SpawnItem(Collection.collectionObjs[i]);
        Destroy(Collection.collectionObjs[i]);
        Collection.collectionObjs.RemoveAt(i);
    }
}
