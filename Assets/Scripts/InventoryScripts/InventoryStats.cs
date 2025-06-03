using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryStats : MonoBehaviour
{
    public InventoryList Inventory;
    public PlayerStats PlayerStats;

    public GameObject storedItem;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
    }

    //updates when a new item enters inventory.
    public void UpdateStats()
    {
        PlayerStats.attack = 0f;
        foreach (var item in Inventory.inventoryList)
        {
            NewItemScript itemScript = item.GetComponent<NewItemScript>();
            if (itemScript != null)
            {
                PlayerStats.attack += itemScript.itemData.damage / itemScript.itemData.cooldown;
            }
        }

        if (storedItem != null)
        {
            NewItemScript storedItemScript = storedItem.GetComponent<NewItemScript>();
            if (storedItemScript != null)
            {
                PlayerStats.attack += storedItemScript.itemData.damage / storedItemScript.itemData.cooldown;
            }
        }
    }
    //get armor at start of combat.
    public void UpdateArmorStats() 
    {
        PlayerStats.armor = 0f;
        foreach (var item in Inventory.inventoryList) 
        {
            NewItemScript itemScript = item.GetComponent<NewItemScript>();

            PlayerStats.armor += itemScript.itemData.armor;

        }
    }

    public float UpdatePoisonStats()
    {
        float poisonAmount = 0f;
        foreach (var item in Inventory.inventoryList)
        {
            NewItemScript itemScript = item.GetComponent<NewItemScript>();

            poisonAmount += itemScript.itemData.poison;
        }
        return poisonAmount;
    }

    public void UpdateThorns()
    {
        PlayerStats.thorns = 0;
        foreach (var item in Inventory.inventoryList)
        {
            NewItemScript itemScript = item.GetComponent<NewItemScript>();

            PlayerStats.thorns += itemScript.itemData.thorns;
        }
    }
}
