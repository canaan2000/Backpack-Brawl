using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyInventoryStats : MonoBehaviour
{
    public EnemyInventoryList Inventory;
    public CombatScript Combat;
    public EnemyScript Enemy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy != null) { UpdateStats(); }
        

        if (Combat.enemy !=  null && Enemy != null) 
        {
            Enemy = Combat.enemy.GetComponent<EnemyScript>();
        }
    }

    //updates when a new item enters inventory.
    public void UpdateStats()
    {
        Enemy.Attack = 2f;
        foreach (var item in Inventory.inventoryList)
        {
            NewItemScript itemScript = item.GetComponent<NewItemScript>();
            if (itemScript != null)
            {
                Enemy.Attack += itemScript.itemData.damage;
            }
        }
    }
    //get armor at start of combat.
    public void UpdateArmorStats() 
    {
        Enemy.Armor = 0f;
        foreach (var item in Inventory.inventoryList) 
        {
            NewItemScript itemScript = item.GetComponent<NewItemScript>();

            Enemy.Armor += itemScript.itemData.armor;

        }
    }
}
