using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryStats : MonoBehaviour
{
    public InventoryList Inventory;
    public PlayerStats PlayerStats;

    Dictionary<string, float> itemDamageDictionary = new Dictionary<string, float>();
    Dictionary<string, float> itemArmorDictionary = new Dictionary<string, float>();
    // Start is called before the first frame update
    void Start()
    {//How much damage items deal while in your inventory.
        itemDamageDictionary.Add("RockTag", 1f);
        itemDamageDictionary.Add("AxeTag", 2f);
        itemDamageDictionary.Add("SwordTag", 6f);

        //How armor items give you at the start of combat.
        itemArmorDictionary.Add("RockTag", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //updates when a new item enters inventory.
    public void UpdateStats()
    {
        PlayerStats.attack = 1f;
        foreach (var item in Inventory.inventoryList)
        {
            if (itemDamageDictionary.ContainsKey(item.tag)) 
            {
                PlayerStats.attack += itemDamageDictionary[item.tag];
            }
        }
    }
    //get armor at start of combat.
    public void UpdateArmorStats() 
    {
        PlayerStats.armor = 0f;
        foreach (var item in Inventory.inventoryList) 
        {
            if (itemArmorDictionary.ContainsKey(item.tag))
            {
                PlayerStats.armor += itemArmorDictionary[item.tag];
            }
        }
    }
}
