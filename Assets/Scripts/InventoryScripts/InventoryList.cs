using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    public InventoryStats Stats;
    public GameObject inventory;
    public List<GameObject> inventoryList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryList != null)
        {
            foreach (var item in inventoryList)
            {
                if (item == null)
                {
                    inventoryList.Remove(item);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //prevent item from being added twice.
        if (!inventoryList.Contains(other.gameObject))
        {
            //add item
            inventoryList.Add(other.gameObject);
        }
        //update playerstats on item enter.
            Stats.UpdateStats();
    }

    public void StartDamageNumbers()
    {
        foreach (var item in inventoryList)
        {
            if (item.GetComponent<NewItemScript>().itemData.damage > 0)
            {
                item.GetComponent<DamageNumberSpawner>().SpawnDamageNumber();
            }
        }
    }
}
