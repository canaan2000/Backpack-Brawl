using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryStats : MonoBehaviour
{
    public InventoryList Inventory;
    public PlayerStats PlayerStats;

    Dictionary<string, float> itemDamageDictionary = new Dictionary<string, float>();
    // Start is called before the first frame update
    void Start()
    {
        itemDamageDictionary.Add("RockTag", 1f);
        itemDamageDictionary.Add("AxeTag", 2f);
        itemDamageDictionary.Add("SwordTag", 6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStats()
    {
        PlayerStats.attack = 1f;
        foreach (var item in Inventory.inventoryList)
        {
            if (itemDamageDictionary.ContainsKey(item.tag)) 
            {
                Debug.Log(itemDamageDictionary[item.tag]);
                PlayerStats.attack += itemDamageDictionary[item.tag];
            }
        }
    }
}
