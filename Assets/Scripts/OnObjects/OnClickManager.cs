using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CombatScript Combat;
    public DamageNumberSpawner NumberSpawner;

    private float clickCooldown = 1f;
    public bool readyToClick = true;
    float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        Combat = GameObject.Find("CombatManager").GetComponent<CombatScript>();

        NumberSpawner = GetComponent<DamageNumberSpawner>();

        cooldown = clickCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0)
        {
            readyToClick = true;
            cooldown = clickCooldown;
        }

        if (Input.GetMouseButtonDown(0))
        {
            readyToClick = false;

        }

        if (!readyToClick)
        {
            cooldown -= Time.deltaTime;
        }

        if (this.tag == "EnemyItem")
        {
            EnemyClick();
        }
    }

    //What an object does when clicked.
    private void OnMouseDown()
    {
        
        NewItemScript itemScript = gameObject.GetComponent<NewItemScript>();
        if (Combat.PlayerStats.stamina >= itemScript.itemData.staminaUsage && Combat.PlayerStats.mana >= itemScript.itemData.clickManaUsage && Combat.combatTrue == true && this.tag != "EnemyItem")
        {
            readyToClick = false;
            Combat.PlayerStats.armor += itemScript.itemData.clickArmor;
            Combat.EnemyStats.Health -= itemScript.itemData.clickDamage;
            Combat.EnemyStats.Poison += itemScript.itemData.clickPoison;
            Combat.PlayerStats.health += itemScript.itemData.clickHealing;

            Combat.PlayerStats.stamina -= itemScript.itemData.staminaUsage;
            Combat.PlayerStats.mana -= itemScript.itemData.clickManaUsage;

            NumberSpawner.OnClickSpawnNumber();
            if (itemScript.itemData.singleUse == true)
            {
                Destroy(gameObject);
            }
        }

    }

    private void EnemyClick()
    {
        NewItemScript itemScript = gameObject.GetComponent<NewItemScript>();

        // Check if the enemy has enough stamina and mana, if combat is active,
        // and if the item is specifically tagged as an "EnemyItem".
        if (Combat.EnemyStats.stamina >= itemScript.itemData.staminaUsage &&
            Combat.EnemyStats.mana >= itemScript.itemData.clickManaUsage &&
            Combat.combatTrue == true &&
            this.tag == "EnemyItem") // Condition changed for enemy item
        {
            // Set readyToClick to false to prevent immediate re-use.
            // (Assuming readyToClick is a member variable of the class this code is in)
            readyToClick = false;

            // Apply item effects to the Player (since it's an enemy item)
            Combat.PlayerStats.armor += itemScript.itemData.clickArmor;
            Combat.PlayerStats.health -= itemScript.itemData.clickDamage; // Enemy deals damage to Player
            Combat.PlayerStats.poison += itemScript.itemData.clickPoison; // Enemy applies poison to Player
            Combat.EnemyStats.Health += itemScript.itemData.clickHealing; // Enemy heals itself

            // Deduct stamina and mana from the Enemy
            Combat.EnemyStats.stamina -= itemScript.itemData.staminaUsage;
            Combat.EnemyStats.mana -= itemScript.itemData.clickManaUsage;

            // Call the number spawner (assuming this is for visual feedback like damage numbers)
            NumberSpawner.OnClickSpawnNumber();

            // If the item is single-use, destroy the GameObject after use.
            if (itemScript.itemData.singleUse == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
