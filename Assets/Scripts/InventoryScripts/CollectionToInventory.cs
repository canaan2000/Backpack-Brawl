using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionToInventory : MonoBehaviour
{
    public CollectionManager Collection;
    public SpawnerScript Spawner;
    public InventoryList InventoryList;
    public CombatScript Combat;

    public int maxInventory;
    
    // Start is called before the first frame update
    void Start()
    {
        maxInventory = Combat.level + 1;
    }

    // Update is called once per frame
    void Update()
    {
        maxInventory = Combat.level + 1;

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
